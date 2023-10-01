namespace TaskManagementSystem.Core.Dto
{
    public record IssueNodeShortDto
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
    }
}