using Microsoft.EntityFrameworkCore;
using OvensCommonLib.Models;

namespace OvensCommonLib.Data
{
    public class OvensDbContext : DbContext
    {
        public DbSet<OvenLog> OvenLogs { get; set; }

        public OvensDbContext(DbContextOptions<OvensDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OvenLog>(entity =>
            {
                // Primary key
                entity.HasKey(l => l.Id);

                // Auto-increment PK
                entity.Property(l => l.Id)
                      .ValueGeneratedOnAdd();

                // Default value for Timestamp
                entity.Property(l => l.Timestamp)
                      .HasDefaultValueSql("GETDATE()");

                // Required fields
                entity.Property(l => l.OvenNumber).IsRequired();
                entity.Property(l => l.CycleStep).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
