using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Core.Abstractions;

namespace TaskManagementSystem.DataAccess.Data
{

    public class DbInitializer : IDbInitializer
    {
        private readonly DbContext _context;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(
            ILogger<DbInitializer> logger,
            DataContext context)
        {
            _context = context;
            _logger = logger;
        }

        public void SeedDb()
        {
            if (_context.Database.EnsureCreated())
            {
                _logger.LogInformation("Database is created.");
                var issues = DataFactory.GetPreconfiguredIssues();
                _context.AddRange(issues);
                _context.SaveChanges();
            }
            else
            {
                _logger.LogInformation("Database already exists.");
            }
        }

        public void MigrateDb()
        {
            if (_context.Database.EnsureCreated())
            {
                _logger.LogInformation("Database is created.");
            }
            else
            {
                _logger.LogInformation("Database already exists.");
                _context.Database.Migrate();
                _logger.LogInformation("Database is migrated.");
            }
        }
    }
}