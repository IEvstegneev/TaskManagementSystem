namespace TaskManagementSystem.Core
{
    public class IssueNode : BaseEntity
    {
        public string Title { get; private set; }
        //public string Description { get; private set; }
        //public string Performers { get; private set; }
        //public IssueStatus Status { get; private set; }
        //public TimeSpan EstimatedLaborCost { get; private set; }
        //public TimeSpan ActualLaborCost { get; private set; }
        //public DateTime CreatedAt { get; private set; }
        //public DateTime StartedAt { get; private set; }
        //public DateTime FinishedAt { get; private set; }

        public bool IsRoot { get; set; }
        public bool IsLeaf { get; set; }
        public ICollection<IssueLink> Descendants { get; private set; } = new List<IssueLink>();
        public ICollection<IssueLink> Ancestors { get; private set; } = new List<IssueLink>();

        public IssueNode(string title, bool isRoot)
        {
            Title = title;
            IsRoot = isRoot;
            IsLeaf = true;
        }
        public IssueNode(string title)
        {
            Title = title;
        }
    }

    public class IssueLink
    {
        public Guid ParentId { get; private set; }
        public Guid ChildId { get; private set; }
        public int Depth { get; private set; }

        public IssueLink(Guid parentId, Guid childId, int depth)
        {
            ParentId = parentId;
            ChildId = childId;
            Depth = depth;
        }
    }
}