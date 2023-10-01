using TaskManagementSystem.Core.Dto;
using TaskManagementSystem.Core.Exceptions;

namespace TaskManagementSystem.Core.Domain
{
    public class IssueNode : BaseEntity
    {
        private TimeSpan _actualLaborCost;
        private TimeSpan _estimatedLaborCost;

        public Guid? ParentId { get; set; }
        public ICollection<IssueNode> Children { get; set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Performers { get; private set; }
        public IssueStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        public TimeSpan EstimatedLaborCost
        {
            get
            {
                var result = _estimatedLaborCost;
                foreach (var child in Children)
                    result += child.EstimatedLaborCost;

                return result;
            }
        }

        public TimeSpan ActualLaborCost
        {
            get
            {
                var result = _actualLaborCost;
                if (Status == IssueStatus.InProgress)
                    result += (DateTime.Now - StartedAt!.Value);

                foreach (var child in Children)
                    result += child.ActualLaborCost;

                return result;
            }
        }

        public bool CanStart => Status == IssueStatus.Assigned || Status == IssueStatus.Stopped;
        public bool CanStop => Status == IssueStatus.InProgress;
        public bool CanFinish
        {
            get
            {
                var result = Status == IssueStatus.InProgress;
                foreach (var child in Children)
                {
                    result &= child.Status == IssueStatus.InProgress 
                        || child.Status == IssueStatus.Finished;
                    if (!result) break;
                }
                return result;
            }
        }

        public IssueNode(
            string title,
            string description,
            string performers,
            TimeSpan estimatedLaborCost,
            Guid? parentId = null)
        {
            Title = title;
            Description = description;
            Performers = performers;
            Status = IssueStatus.Assigned;
            CreatedAt = DateTime.Now;
            _estimatedLaborCost = estimatedLaborCost;
            ParentId = parentId;
            Children = new List<IssueNode>();
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
                _estimatedLaborCost = TimeSpan.FromHours(data.EstimatedLaborCost.Value);
        }

        public void Start()
        {
            if (CanStart)
            {
                Status = IssueStatus.InProgress;
                StartedAt = DateTime.Now;
            }
            else
                throw new StartIssueException($"Cannot start the issue. Actual Status is {Status}");
        }

        public void Stop()
        {
            if (CanStop)
            {
                Status = IssueStatus.Stopped;
                _actualLaborCost += DateTime.Now - StartedAt!.Value;
                StartedAt = null;
            }
            else
                throw new StopIssueException($"Cannot stop the issue. Actual Status is {Status}");
        }

        public void Finish()
        {
            if (CanFinish)
            {
                foreach (var child in Children)
                    child.Finish();

                Status = IssueStatus.Finished;
                _actualLaborCost += DateTime.Now - StartedAt!.Value;
                FinishedAt = DateTime.Now;
            }
            else
                throw new StopIssueException($"Cannot finish the issue. Actual Status is {Status}");
        }
    }
}