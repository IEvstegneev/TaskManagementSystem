namespace TaskManagementSystem.Core.Dto
{
    public record UpdateIssueDto
    {
        public string? Title { get; init; }
        public string? Description { get; init; }
        public string? Performers { get; init; }
        public double? EstimatedLaborCost { get; init; }
    }
}
