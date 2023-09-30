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

        public async Task<IssueNode[]> GetRootNodesListAsync()
        {
            var issues = await _context.IssueNodes
                .Where(x => x.IsRoot)
                .ToArrayAsync();

            return issues;
        }

        public async Task<IssueNode[]> GetChildrenListAsync(Guid id)
        {
            var node = await _context.IssueNodes
                .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (node == null)
                throw new Exception("Not found");

            return node.Children.ToArray();
        }


        public async Task<IssueNode?> GetNodeAsync(Guid id)
        {
            var node = await _context.IssueNodes
                .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (node == null)
                throw new Exception("Not found");

            return node;
        }

        public async Task<Guid> CreateNodeAsync(IssueNode node, Guid? parentId = null)
        {
            if (parentId.HasValue)
            {
                var parent = await _context.IssueNodes
                    .FirstOrDefaultAsync(x => x.Id == parentId);

                if (parent == null)
                    throw new Exception("Not found");

                parent.IsLeaf = false;
            }
            else
                node.IsRoot = true;

            node.IsLeaf = true;
            _context.IssueNodes.Add(node);
            await _context.SaveChangesAsync();
            return node.Id;
        }

        public async Task<Guid> UpdateNodeAsync(Guid id, IssueNode data)
        {
            var node = await _context.IssueNodes.FindAsync(id);

            if (node == null)
                throw new Exception();

            // refact
            node.Update(data.Title);

            await _context.SaveChangesAsync();
            return node.Id;
        }

        public async Task DeleteNodeAsync(Guid id)
        {
            var node = await _context.IssueNodes.FindAsync(id);

            if (node != null)
                _context.IssueNodes.Remove(node);

            await _context.SaveChangesAsync();
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
    }
}
