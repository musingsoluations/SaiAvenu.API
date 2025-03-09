using Microsoft.EntityFrameworkCore;
using SriSai.Domain.Entity.Building;
using SriSai.Domain.Entity.Users;

namespace SriSai.infrastructure.Persistent.DbContext;

public class SriSaiDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public SriSaiDbContext(DbContextOptions<SriSaiDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<Apartment> Apartments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SriSaiDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}