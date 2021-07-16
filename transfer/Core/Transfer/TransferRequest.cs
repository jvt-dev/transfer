using System.ComponentModel.DataAnnotations;

namespace transfer.Core.Transfer
{
    public class TransferRequest
    {
        [Required]
        public string AccountOrigin { get; set; }

        [Required]
        public string AccountDestination { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}
