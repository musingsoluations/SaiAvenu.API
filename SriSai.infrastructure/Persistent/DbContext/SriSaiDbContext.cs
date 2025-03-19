using Microsoft.EntityFrameworkCore;
using SriSai.Domain.Entity.Building;
using SriSai.Domain.Entity.Collection;
using SriSai.Domain.Entity.Users;

namespace SriSai.infrastructure.Persistent.DbContext
{
    public class SriSaiDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public SriSaiDbContext(DbContextOptions<SriSaiDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ApartmentEntity> Apartments { get; set; }
        public DbSet<FeeCollectionEntity> FeeCollections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SriSaiDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}