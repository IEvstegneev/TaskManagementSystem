using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Abstractions;
using TaskManagementSystem.Core.Domain;
using TaskManagementSystem.Core.Dto;
using TaskManagementSystem.DataAccess.Data;

namespace TaskManagementSystem.DataAccess
{
    public class IssuesService : IIssuesService
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

        public async Task<OperationResult<IssueNodeDto>> GetIssueAsync(Guid id)
        {
            var issue = await _context.IssueNodes
                .FirstOrDefaultAsync(x => x.Id == id);

            if (issue == null)
                return OperationResult<IssueNodeDto>.NotFoundById(id);

            await _context.IssueNodes.LoadAsync();
            var dto = issue.ToIssueNodeDto();
            return OperationResult<IssueNodeDto>.Ok(dto);
        }

        public async Task<OperationResult<Guid>> CreateNodeAsync(CreateIssueDto dto, Guid? parentId = null)
        {
            if (parentId.HasValue)
            {
                var parent = await _context.IssueNodes.FindAsync(parentId);
                if (parent == null)
                    return OperationResult<Guid>.NotFoundById(parentId.Value);
            }

            var node = dto.ToIssueNode();
            _context.IssueNodes.Add(node);
            await _context.SaveChangesAsync();
            return OperationResult<Guid>.Ok(node.Id);
        }

        public async Task<OperationResult<Guid>> UpdateNodeAsync(Guid id, UpdateIssueDto data)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node != null)
            {
                node.Update(data);
                await _context.SaveChangesAsync();
                return OperationResult<Guid>.Ok(node.Id);
            }
            return OperationResult<Guid>.NotFoundById(id);
        }


        public async Task<OperationResult> DeleteNodeAsync(Guid id)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node != null)
            {
                _context.IssueNodes.Remove(node);
                await _context.SaveChangesAsync();
                return OperationResult.Ok();
            }
            return OperationResult.NotFoundById(id);
        }

        public async Task<OperationResult> MoveNodeAsync(Guid id, Guid to)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node == null)
                return OperationResult.NotFoundById(id);

            var newParent = await _context.IssueNodes.FindAsync(to);
            if (newParent == null)
                return OperationResult.NotFoundById(id);

            await _context.IssueNodes.LoadAsync();
            if (node.Contains(newParent))
                return OperationResult.CannotMoveToDescendant();

            node.ChangeParent(to);
            await _context.SaveChangesAsync();
            return OperationResult.Ok();
        }

        public async Task<OperationResult> MoveNodeToRootAsync(Guid id)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node == null)
                return OperationResult.NotFoundById(id);

            node.ResetParent();
            await _context.SaveChangesAsync();
            return OperationResult.Ok();
        }

        public async Task<OperationResult> ChangeStatusAsync(Guid id, IssueStatus status)
        {
            var issue = await _context.IssueNodes.FirstOrDefaultAsync(x => x.Id == id);
            if (issue == null)
                return OperationResult.NotFoundById(id);

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
            return OperationResult.Ok();
        }
    }
}
