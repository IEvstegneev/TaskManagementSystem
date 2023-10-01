using TaskManagementSystem.Core.Dto;
using TaskManagementSystem.Core.Exceptions;

namespace TaskManagementSystem.Core.Domain
{
    public class IssueNode : BaseEntity
    {
        private TimeSpan _actualLaborCost;

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Performers { get; private set; }
        public IssueStatus Status { get; private set; }
        public TimeSpan EstimatedLaborCost { get; private set; }
        public TimeSpan ActualLaborCost =>
            Status == IssueStatus.InProgress
                ? _actualLaborCost + (DateTime.Now - StartedAt!.Value)
                : _actualLaborCost;

        public DateTime CreatedAt { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }

        public bool IsRoot { get; set; }
        public bool IsLeaf { get; set; }
        public Guid? ParentId { get; set; }
        public ICollection<IssueNode> Children { get; set; }

        public IssueNode(string title, Guid? parentId = null)
        {
            Title = title;

            ParentId = parentId;
        }

        public IssueNode(
            string title,
            string description,
            string performers,
            TimeSpan estimatedLaborCostInHours,
            Guid? parentId = null) : this(title)
        {
            Title = title;
            Description = description;
            Performers = performers;
            Status = IssueStatus.Assigned;
            CreatedAt = DateTime.Now;
            EstimatedLaborCost =  estimatedLaborCostInHours;
            ParentId = parentId;
        }

        public void Update(UpdateIssueDto data)
        {
            if (data.Title != null)
                Title = data.Title;
            if (data.Description != null)
                Description = data.Description;
            if (data.Performers != null)
                Performers = data.Performers;
            if (data.EstimatedLaborCost != null)
                EstimatedLaborCost = TimeSpan.FromHours(data.EstimatedLaborCost.Value);
        }

        public void Start()
        {
            if (Status == IssueStatus.Assigned || Status == IssueStatus.Stopped)
            {
                Status = IssueStatus.InProgress;
                StartedAt = DateTime.Now;
            }
            else
                throw new StartIssueException($"Cannot start the issue. Actual Status is {Status}");
        }

        public void Stop()
        {
            if (Status == IssueStatus.InProgress && StartedAt != null)
            {
                Status = IssueStatus.Stopped;
                _actualLaborCost += DateTime.Now - StartedAt.Value;
                StartedAt = null;
            }
            else
                throw new StopIssueException($"Cannot stop the issue. Actual Status is {Status}");
        }

        public void Finish()
        {
            if (Status == IssueStatus.InProgress && StartedAt != null)
            {
                Status = IssueStatus.Finished;
                _actualLaborCost += DateTime.Now - StartedAt.Value;
                StartedAt = null;
                FinishedAt = DateTime.Now;
            }
            else
                throw new StopIssueException($"Cannot finish the issue. Actual Status is {Status}");
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