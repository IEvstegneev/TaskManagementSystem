namespace TaskManagementSystem.WebApi.Dto
{
    internal record IssueNodeShortDto
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public bool IsRoot { get; init; }
        public bool IsLeaf { get; init; }
    }
}