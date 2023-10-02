using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementSystem.Core.Domain;

namespace TaskManagementSystem.DataAccess.Data
{
    internal class IssueNodeConfiguration : IEntityTypeConfiguration<IssueNode>
    {
        public void Configure(EntityTypeBuilder<IssueNode> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.Description)
                .HasMaxLength(2000)
                .IsRequired();
            builder.Property(x => x.Performers)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.Status)
                .IsRequired();
            builder.Property(x => x.EstimatedLaborCost)
                .HasField("_estimatedLaborCost")
                .IsRequired();
            builder.Property(x => x.ActualLaborCost)
                .HasField("_actualLaborCost")
                .IsRequired();
            builder.Property(x => x.CreatedAt)
                .IsRequired();
            builder.Property(x => x.StartedAt)
                .IsRequired(false);
            builder.Property(x => x.FinishedAt)
                .IsRequired(false);

            builder.HasMany(node => node.Children)
                .WithOne()
                .HasForeignKey(node => node.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}