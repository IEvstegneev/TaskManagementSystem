using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core;

namespace TaskManagementSystem.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<IssueNode> Phrases { get; private set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}