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
                .Where(x => x.ParentId == null)
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
            }

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

        public async Task MoveNodeAsync(Guid id, Guid to)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node == null)
            {
                throw new NotFoundException();
            }
            var newParent = await _context.IssueNodes.FindAsync(to);
            if (newParent == null)
            {
                throw new NotFoundException();
            }
            node.ParentId = to;
            await _context.SaveChangesAsync();
        }

        public async Task MoveNodeToRootAsync(Guid id)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node != null)
            {
                node.ParentId = null;
                await _context.SaveChangesAsync();
            }
            else
            {

            }
        }

        public async Task<IssueStatus?> ChangeStatusAsync(Guid id, IssueStatus status)
        {
            var issue = await _context.IssueNodes
                .FirstOrDefaultAsync(x => x.Id == id);

            if (issue != null)
            {
                if (status == IssueStatus.InProgress)
                    issue.Start();

                if (status == IssueStatus.Stopped)
                    issue.Stop();

                if (status == IssueStatus.Finished)
                {
                    await _context.IssueNodes.LoadAsync();
                    issue.Finish();
                }

                await _context.SaveChangesAsync();
                return issue.Status;
            }
            return null;
        }
    }
}
