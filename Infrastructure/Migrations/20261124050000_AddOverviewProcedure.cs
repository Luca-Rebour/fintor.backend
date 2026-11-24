using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

[DbContext(typeof(FintorDbContext))]
[Migration("20261124050000_AddOverviewProcedure")]
public sealed class AddOverviewProcedure : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        using var stream = typeof(AddOverviewProcedure).Assembly.GetManifestResourceStream(
            "Infrastructure.Sql.usp_GetOverview.sql")
            ?? throw new InvalidOperationException("Missing embedded overview procedure SQL.");
        using var reader = new StreamReader(stream);
        // Dynamic SQL lets CREATE PROCEDURE be the first statement in its batch,
        // including when EF wraps it in an idempotent migration guard.
        migrationBuilder.Sql("EXEC(N'" + reader.ReadToEnd().Replace("'", "''") + "');");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_GetOverview;");
}
