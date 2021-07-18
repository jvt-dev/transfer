namespace transfer.Core.Services
{
    public class AccountServiceRequest
    {
        public string AccountNumber { get; set; }
        public decimal Value { get; set; }
        public string Type { get; set; } = "Credit";
    }
}
