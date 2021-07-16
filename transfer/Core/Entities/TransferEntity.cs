using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace transfer.Core.Entities
{
    [Table("transfer")]
    public class Transfer
    {
        [Key]
        [Column("idtransfer")]
        public int IdTransfer { get; set; }

        [Column("accountorigin")]
        public string AccountOrigin { get; set; }

        [Column("accountdestination")]
        public string AccountDestination { get; set; }

        [Column("idtransferstatus")]
        public int IdTransferStatus { get; set; }

        [ForeignKey("IdTransferStatus")]
        public TransferStatus TransferStatus { get; set; }

        [Column("transfervalue")]
        public decimal TransferValue { get; set; }
    }
}
