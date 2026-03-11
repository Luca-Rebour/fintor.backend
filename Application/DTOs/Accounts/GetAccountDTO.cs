namespace Application.DTOs.Accounts
{
    public class GetAccountDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public GetAccountsCurrencyResponseDTO Currency { get; set; } = new GetAccountsCurrencyResponseDTO();
        public string Icon { get; set; } = string.Empty;
        public decimal TotalBalance { get; set; }
        public decimal AvailableBalance { get; set; }
    }

    public class GetAccountsCurrencyResponseDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
