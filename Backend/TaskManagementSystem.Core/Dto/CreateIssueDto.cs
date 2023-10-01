namespace TaskManagementSystem.Core.Dto
{
    public record CreateIssueDto
    {
        public Guid? ParentId { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string Performers { get; init; }
        public required double EstimatedLaborCost { get; init; }
    }
}
