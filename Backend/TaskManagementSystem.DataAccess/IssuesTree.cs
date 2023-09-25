using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core;

namespace TaskManagementSystem.DataAccess
{
    public class IssuesTree
    {
        private readonly DataContext _context;

        public IssuesTree(DataContext context)
        {
            _context = context;
        }

        public async Task<IssueNode[]> GetRootNodesAsync()
        {
            var issues = await _context.IssueNodes
                .Where(x => x.IsRoot)
                .ToArrayAsync();

            return issues;
        }

        public async Task<IssueNode?> GetNodeAsync(Guid id)
        {
            var issueNode = await _context.IssueNodes
                .FirstAsync(x => x.Id == id);
            return issueNode;
        }

        public async Task<IssueNode[]> GetDescendantsAsync(Guid parentId)
        {
            var descendantsIds = _context.IssueLinks
                 .AsNoTracking()
                 .Where(x => x.ParentId == parentId)
                 .Where(x => x.ParentId != x.ChildId)
                 .Select(x => x.ChildId);

            var descendants = await _context.IssueNodes
                .AsNoTracking()
                .Join(descendantsIds, x => x.Id, x => x, (x, y) => x)
                //.Select(x => x.Title)
                .ToArrayAsync();

            return descendants;
        }

        public async Task<Guid> CreateNodeAsync(IssueNode node, Guid? parentId = null)
        {
            if (parentId.HasValue && await _context.IssueNodes.FindAsync(parentId) != null)
            {
                await MakeLinksAsync(node.Id, parentId.Value);
            }
            else
                throw new ArgumentException(); // not found

            _context.IssueLinks.Add(new IssueLink(node.Id, node.Id, 0));
            _context.IssueNodes.Add(node);
            await _context.SaveChangesAsync();
            return node.Id;
        }

        public async Task MoveNodeAsync(Guid id, Guid? to = null)
        {
            var node = await _context.IssueNodes.FindAsync(id);
            if (node != null)
            {
                BreakLinks(node);
                if (to.HasValue)
                {
                    await MakeLinksAsync(id, to.Value);
                    node.IsRoot = false;
                }
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Make links from the issue to all their ancestors.
        /// </summary>
        /// <param name="from">Issue id.</param>
        /// <param name="to">Parent issue id.</param>
        private async Task MakeLinksAsync(Guid from, Guid to)
        {
            await _context.IssueLinks
                .AsNoTracking()
                .Where(c => c.ChildId == to)
                .Select(x => new { x.ParentId, x.Depth })
                .ForEachAsync(x
                    => _context.Add(new IssueLink(x.ParentId, from, x.Depth + 1)));
        }

        private void BreakLinks(IssueNode node)
        {
            var oldLinks = _context.IssueLinks
                .Where(x => x.ChildId == node.Id)
                .Where(x => x.Depth > 0);
            _context.IssueLinks.RemoveRange(oldLinks);
            node.IsRoot = true;
        }
    }
}
