using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core.Domain;

namespace TaskManagementSystem.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<IssueNode> IssueNodes { get; private set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IssueNodeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}