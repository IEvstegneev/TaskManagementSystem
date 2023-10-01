using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Domain;
using TaskManagementSystem.Core.Dto;
using TaskManagementSystem.Core.Exceptions;

namespace TaskManagementSystem.DataAccess
{
    public class IssuesService
    {
        private readonly DataContext _context;

        public IssuesService(DataContext context)
        {
            _context = context;
        }

        public async Task<IssueNodeShortDto[]> GetRootIssuesListAsync()
        {
            var issues = await _context.IssueNodes
                .AsNoTracking()
                .Where(x => x.IsRoot)
                .Select(x => new IssueNodeShortDto { Id = x.Id, Title = x.Title })
                .ToArrayAsync();

            return issues;
        }

        public async Task<IssueNodeShortDto[]> GetChildrenListAsync(Guid id)
        {
            var children = await _context.IssueNodes
                .AsNoTracking()
                .Where(x => x.ParentId == id)
                .Select(x => new IssueNodeShortDto { Id = x.Id, Title = x.Title })
                .ToArrayAsync();

            return children;
        }

        public async Task<IssueNodeDto?> GetIssueAsync(Guid id)
        {
            var issue = await _context.IssueNodes
                .FirstOrDefaultAsync(x => x.Id == id);
            //var issue = await _context.IssueNodes
            //    .Where(x => x.Id == id)
            //    .FirstOrDefaultAsync();

            if (issue == null)
                return null;

            await _context.IssueNodes.LoadAsync();

            return issue.ToIssueNodeDto();
        }

        public async Task<Guid?> CreateNodeAsync(CreateIssueDto dto, Guid? parentId = null)
        {
            var node = dto.ToIssueNode();
            if (parentId.HasValue)
            {
                var parent = await _context.IssueNodes.FindAsync(parentId);
                if (parent == null)
                    return null;

                parent.IsLeaf = false;
            }
            else
                node.IsRoot = true;

            node.IsLeaf = true;
            _context.IssueNodes.Add(node);
            await _context.SaveChangesAsync();
            return node.Id;
        }

        public async Task<Guid?> UpdateNodeAsync(Guid id, UpdateIssueDto data)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node != null)
            {
                node.Update(data);
                await _context.SaveChangesAsync();
                return node.Id;
            }
            return null;
        }


        public async Task DeleteNodeAsync(Guid id)
        {
            var node = await _context.IssueNodes.FindAsync(id);

            if (node != null)
            {
                _context.IssueNodes.Remove(node);
                await _context.SaveChangesAsync();
            }
        }



        public async Task MoveNodeAsync(Guid id, Guid? to = null)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node != null)
            {
                if (to.HasValue)
                {
                    var newParent = await _context.IssueNodes.FindAsync(to);
                    if (newParent != null)
                    {
                        //if (node.ParentId.HasValue)
                        //{
                        //    var oldParent = await _context.IssueNodes.FindAsync(node.ParentId);
                        //}
                        node.ParentId = to;
                        newParent.IsLeaf = false;
                        node.IsRoot = false;
                    }
                    else
                        throw new ArgumentException(); // not found
                }
                else
                {
                    node.ParentId = null;
                    node.IsRoot = true;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IssueStatus?> ChangeStatusAsync(Guid id, IssueStatus status)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node != null)
            {
                switch (status)
                {
                    case IssueStatus.InProgress: node.Start();
                        break;
                    case IssueStatus.Stopped: node.Stop();
                        break;
                    case IssueStatus.Finished: node.Finish();
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync();
                return node.Status;
            }
            return null;
        }
    }
}
