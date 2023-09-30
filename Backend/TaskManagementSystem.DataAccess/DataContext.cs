using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementSystem.Core;

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

    internal class IssueNodeConfiguration : IEntityTypeConfiguration<IssueNode>
    {
        public void Configure(EntityTypeBuilder<IssueNode> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IsRoot)
                .IsRequired();

            builder.HasMany(node => node.Children)
                .WithOne()
                .HasForeignKey(node => node.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}