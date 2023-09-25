using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementSystem.Core;

namespace TaskManagementSystem.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<IssueNode> IssueNodes { get; private set; }
        public DbSet<IssueLink> IssueLinks { get; private set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IssueNodeConfiguration());
            modelBuilder.ApplyConfiguration(new ClosureConfiguration());
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

            //builder.HasMany(parent => parent.Children)
            //    .WithOne()
            //    .HasForeignKey(node => node.ParentId)
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Descendants)
                .WithOne()
                .HasForeignKey(x => x.ParentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Ancestors)
                .WithOne()
                .HasForeignKey(x => x.ChildId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    internal class ClosureConfiguration : IEntityTypeConfiguration<IssueLink>
    {
        public void Configure(EntityTypeBuilder<IssueLink> builder)
        {
            builder.HasKey(closure => new { closure.ParentId, closure.ChildId });
            builder.Property(x => x.Depth)
                .HasDefaultValue(0)
                .IsRequired();
            //builder.HasOne(x => x.Parent)
            //    .WithMany()
            //    .HasForeignKey(x => x.ParentId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(x => x.Child)
            //    .WithMany()
            //    .HasForeignKey(x => x.ChildId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}