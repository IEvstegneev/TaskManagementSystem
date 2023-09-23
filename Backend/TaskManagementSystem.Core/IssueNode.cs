namespace TaskManagementSystem.Core
{
    public class IssueNode : BaseEntity
    {

        public string Name { get; private set; }
        //public string Description { get; private set; }
        //public string Performers { get; private set; }
        //public IssueStatus Status { get; private set; }
        //public TimeSpan EstimatedLaborCost { get; private set; }
        //public TimeSpan ActualLaborCost { get; private set; }
        //public DateTime CreatedAt { get; private set; }
        //public DateTime StartedAt { get; private set; }
        //public DateTime FinishedAt { get; private set; }


        public IssueNode(string name)
        {
            Name = name;
        }
    }
}