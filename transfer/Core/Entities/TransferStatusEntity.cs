using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace transfer.Core.Entities
{
    [Table("transferstatus")]
    public class TransferStatus
    {
        [Key]
        [Column("idtransferstatus")]
        public int IdTransferStatus { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }
}
