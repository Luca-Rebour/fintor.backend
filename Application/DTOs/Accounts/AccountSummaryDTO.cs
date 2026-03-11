namespace Application.DTOs.Accounts
{
    public class AccountSummaryDTO
    {
        public decimal Balance { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public string CurrencySymbol { get; set; } = string.Empty;
    }
}
