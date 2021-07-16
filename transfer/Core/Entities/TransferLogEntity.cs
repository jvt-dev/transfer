using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace transfer.Core.Entities
{
    [Table("transferlog")]
    public class TransferLogEntity
    {
        [Key]
        [Column("idtransferlog")]
        public int IdTransferLog { get; set; }

        [Column("idtransfer")]
        public int IdTransfer { get; set; }

        [ForeignKey("IdTransfer")]
        public TransferEntity Transfer { get; set; }

        [Column("logmessage")]
        public string LogMessage { get; set; }
    }
}
