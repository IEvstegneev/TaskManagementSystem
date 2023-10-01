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
                estimatedLaborCost: TimeSpan.FromHours(dto.EstimatedLaborCost),
                parentId: dto.ParentId);
        }

        public static IssueNodeDto ToIssueNodeDto(this IssueNode issue)
        {
            var dto = new IssueNodeDto
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                Performers = issue.Performers,
                Status = issue.Status,
                EstimatedLaborCost = LaborCostFormat(issue.EstimatedLaborCost),
                ActualLaborCost = LaborCostFormat(issue.ActualLaborCost),
                CreatedAt = issue.CreatedAt.ToString(),
                FinishedAt = issue.FinishedAt?.ToString(),
                Children = issue.Children.Select(x => new IssueNodeChildDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Status = x.Status,
                    EstimatedLaborCost = LaborCostFormat(x.EstimatedLaborCost),
                    ActualLaborCost = LaborCostFormat(x.ActualLaborCost),
                }).ToArray(),
                CanStart = issue.CanStart,
                CanStop = issue.CanStop,
                CanFinish = issue.CanFinish
            };

            return dto;
        }

        private static string LaborCostFormat(TimeSpan time)
            => $"{(time.Days > 0 ? time.Days + " дн" : "")} "
            + $"{Math.Round(time.Hours + time.Minutes / 60.0, 2)} ч";
    }
}
