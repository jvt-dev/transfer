using Microsoft.EntityFrameworkCore;
using transfer.Core.Entities;

namespace transfer.Infrastructure.Data
{
    public class TransferContext : DbContext
    {
        public TransferContext(DbContextOptions<TransferContext> options) : base(options) { }
        public DbSet<TransferStatusEntity> TransferStatus { get; set; }
        public DbSet<TransferEntity> Transfer { get; set; }
        public DbSet<TransferLogEntity> TransferLog { get; set; }
    }
}
