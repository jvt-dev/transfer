using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace transfer.Core.Entities
{
    [Table("transferstatus")]
    public class TransferStatusEntity
    {
        [Key]
        [Column("idtransferstatus")]
        public int IdTransferStatus { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }
}
