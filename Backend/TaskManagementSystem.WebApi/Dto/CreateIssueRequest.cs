namespace TaskManagementSystem.WebApi.Dto
{
    public record CreateIssueRequest
    {
        public Guid? ParentId { get; init; }
        public required string Title { get; init; }
    }
}
