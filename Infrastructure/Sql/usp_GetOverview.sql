CREATE OR ALTER PROCEDURE dbo.usp_GetOverview
    @UserId uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Now datetime2 = SYSUTCDATETIME();

    -- Materialize only this user's transactions. ExchangeRate is the rate to
    -- the user's base currency; same-currency transactions may omit it.
    SELECT t.CategoryId, c.Name AS CategoryName, c.Color AS CategoryColor,
           t.TransactionType, t.[Date],
           CAST(t.Amount * COALESCE(t.ExchangeRate, 1) AS decimal(38, 6)) AS Amount
    INTO #UserTransactions
    FROM dbo.Transactions AS t
    INNER JOIN dbo.Accounts AS a ON a.Id = t.AccountId
    INNER JOIN dbo.Categories AS c ON c.Id = t.CategoryId
    WHERE a.UserId = @UserId AND t.[Date] <= @Now;

    SELECT periods.DaysAgo,
           CAST(COALESCE(totals.TotalIncome, 0) - COALESCE(totals.TotalExpense, 0) AS decimal(18, 2)) AS TotalBalance,
           CAST(COALESCE(totals.TotalIncome, 0) AS decimal(18, 2)) AS TotalIncome,
           CAST(COALESCE(totals.TotalExpense, 0) AS decimal(18, 2)) AS TotalExpense,
           (SELECT t.CategoryId, t.CategoryName, t.CategoryColor,
                   CAST(SUM(t.Amount) AS decimal(18, 2)) AS Total
            FROM #UserTransactions AS t
            WHERE t.TransactionType = 1
              AND t.[Date] >= DATEADD(day, -periods.DaysAgo, @Now)
            GROUP BY t.CategoryId, t.CategoryName, t.CategoryColor
            ORDER BY SUM(t.Amount) DESC, t.CategoryId
            FOR JSON PATH) AS CategorySpendingJson,
           (SELECT t.CategoryId, t.CategoryName, t.CategoryColor,
                   CAST(SUM(t.Amount) AS decimal(18, 2)) AS Total
            FROM #UserTransactions AS t
            WHERE t.TransactionType = 0
              AND t.[Date] >= DATEADD(day, -periods.DaysAgo, @Now)
            GROUP BY t.CategoryId, t.CategoryName, t.CategoryColor
            ORDER BY SUM(t.Amount) DESC, t.CategoryId
            FOR JSON PATH) AS CategoryEarningJson
    FROM (VALUES (7), (30), (182), (365)) AS periods(DaysAgo)
    OUTER APPLY (
        SELECT SUM(CASE WHEN t.TransactionType = 0 THEN t.Amount ELSE 0 END) AS TotalIncome,
               SUM(CASE WHEN t.TransactionType = 1 THEN t.Amount ELSE 0 END) AS TotalExpense
        FROM #UserTransactions AS t
        WHERE t.[Date] >= DATEADD(day, -periods.DaysAgo, @Now)
    ) AS totals
    ORDER BY periods.DaysAgo;
END;
