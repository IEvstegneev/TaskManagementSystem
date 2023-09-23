namespace TaskManagementSystem.WebApi.Dto
{
    public record CreateIssueRequest
    {
        public required string Name { get; init; }
    }
}
