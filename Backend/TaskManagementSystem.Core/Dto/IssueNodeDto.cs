using TaskManagementSystem.Core.Domain;

namespace TaskManagementSystem.Core.Dto
{
    public record IssueNodeDto
    {
        public bool IsRoot { get; init; }
        public bool IsLeaf { get; init; }
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string Performers { get; init; }
        public IssueStatus Status { get; init; }
        public required string CreatedAt { get; init; }
        public string? FinishedAt { get; init; }
        public required string EstimatedLaborCost { get; init; }
        public required string ActualLaborCost { get; init; }
        public IssueNodeChildDto[]? Children { get; init; }
        public bool CanStart { get; init; }
        public bool CanStop { get; init; }
        public bool CanFinish { get; init; }

    }
}