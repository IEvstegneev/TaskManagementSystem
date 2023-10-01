using TaskManagementSystem.Core.Domain;
using TaskManagementSystem.Core.Dto;

namespace TaskManagementSystem.Core
{
    public static class MappingExtentions
    {
        public static IssueNode ToIssueNode(this CreateIssueDto dto)
        {
            return new IssueNode(
                title: dto.Title,
                description: dto.Description,
                performers: dto.Performers,
                estimatedLaborCostInHours: TimeSpan.FromHours(dto.EstimatedLaborCost),
                parentId: dto.ParentId);
        }

        public static IssueNodeDto ToIssueNodeDto(this IssueNode issue)
        {
            var dto = new IssueNodeDto
            {
                IsLeaf = issue.IsLeaf,
                IsRoot = issue.IsRoot,
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                Performers = issue.Performers,
                Status = issue.Status,
                EstimatedLaborCost = issue.EstimatedLaborCost.ToString("h':'mm':'ss"),
                ActualLaborCost = issue.ActualLaborCost.ToString("h':'mm':'ss"),
                //Math.Round(issue.ActualLaborCost.TotalHours, 1),
                CreatedAt = issue.CreatedAt.ToString(),
                FinishedAt = issue.FinishedAt?.ToString(),
                Children = issue.Children.Select(x => new IssueNodeShortDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    IsLeaf = x.IsLeaf,
                    IsRoot = x.IsRoot
                }).ToArray()
            };

            return dto;
        }
    }
}
