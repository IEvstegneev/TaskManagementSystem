namespace TaskManagementSystem.WebApi.Dto
{
    public record UpdateIssueRequest
    {
        public string Title { get; init; } = null!;
    }
}
