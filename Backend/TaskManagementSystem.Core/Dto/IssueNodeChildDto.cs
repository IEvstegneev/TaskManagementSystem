using TaskManagementSystem.Core.Domain;

namespace TaskManagementSystem.Core.Dto
{
    public record IssueNodeChildDto
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public IssueStatus Status { get; init; }
        public required string EstimatedLaborCost { get; init; }
        public required string ActualLaborCost { get; init; }
    }
}