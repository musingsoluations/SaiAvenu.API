using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SriSai.infrastructure.Persistent.DbContext
{
    public class SriSaiDbContextFactory : IDesignTimeDbContextFactory<SriSaiDbContext>
    {
        public SriSaiDbContext CreateDbContext(string[] args)
        {
            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<SriSaiDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new SriSaiDbContext(optionsBuilder.Options);
        }
    }

}