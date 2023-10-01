namespace TaskManagementSystem.Core.Domain
{
    public readonly record struct UpdateIssueParams
    {
        public UpdateIssueParams(
            string? title = null,
            string? description = null,
            string? performers = null,
            TimeSpan? estimatedLaborCost = null)
        {
            Title = title;
            Description = description;
            Performers = performers;
            EstimatedLaborCost = estimatedLaborCost;
        }

        public string? Title { get; init; }
        public string? Description { get; init; }
        public string? Performers { get; init; }
        public TimeSpan? EstimatedLaborCost { get; init; }
    }
}
