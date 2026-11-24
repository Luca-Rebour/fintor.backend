using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

// Offline validation: never opens a database connection.
using var db = new FintorDbContextFactory().CreateDbContext([]);
var migrations = db.Database.GetMigrations().ToArray();
Check(migrations.Length == 2 && migrations[0].EndsWith("_InitialCreate")
    && migrations[1].EndsWith("_AddOverviewProcedure"), "ordered schema and procedure migrations");
Check(!db.Database.HasPendingModelChanges(), "snapshot matches the current model");
var migrator = db.GetService<IMigrator>();
var up = migrator.GenerateScript(options: MigrationsSqlGenerationOptions.Idempotent);
foreach (var table in new[] { "Users", "Accounts", "Currencies", "Categories", "Goals", "Transactions",
    "RecurringTransactions", "PendingAprovalTransactions", "Notifications", "PushSubscriptions" })
    Check(up.Contains($"CREATE TABLE [{table}]"), $"creates {table}");
Check(up.Contains("USD") && up.Contains("UYU") && up.Contains("EUR"), "currency seed data");
Check(up.Contains("EXEC(N'CREATE OR ALTER PROCEDURE dbo.usp_GetOverview"), "procedure uses its own SQL batch");
Check(up.Contains("WHERE a.UserId = @UserId"), "overview scoped to the requested user");
Check(up.Contains("(7), (30), (182), (365)"), "all four report periods");
Check(up.Contains("CategorySpendingJson") && up.Contains("CategoryEarningJson") && up.Contains("CategoryColor"),
    "overview result matches the API contract");
Check(up.Contains("IF NOT EXISTS") && up.Contains("__EFMigrationsHistory"), "idempotent history guards");
var down = migrator.GenerateScript(migrations[^1], Migration.InitialDatabase);
Check(down.IndexOf("DROP PROCEDURE IF EXISTS") < down.IndexOf("DROP TABLE"), "rollback drops procedure before tables");
Check(down.Contains("DROP TABLE [Currencies]"), "rollback covers the initial schema");
Console.WriteLine("All migration checks passed (SQL generation only; no SQL Server connection).");

static void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    Console.WriteLine($"PASS {message}");
}
