using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Domain;
using TaskManagementSystem.Core.Dto;

namespace TaskManagementSystem.Core.Abstractions
{
    public interface IIssuesService
    {
        Task<OperationResult> ChangeStatusAsync(Guid id, IssueStatus status);
        Task<OperationResult<Guid>> CreateNodeAsync(CreateIssueDto dto, Guid? parentId = null);
        Task<OperationResult> DeleteNodeAsync(Guid id);
        Task<IssueNodeShortDto[]> GetChildrenListAsync(Guid id);
        Task<OperationResult<IssueNodeDto>> GetIssueAsync(Guid id);
        Task<IssueNodeShortDto[]> GetRootIssuesListAsync();
        Task<OperationResult> MoveNodeAsync(Guid id, Guid to);
        Task<OperationResult> MoveNodeToRootAsync(Guid id);
        Task<OperationResult<Guid>> UpdateNodeAsync(Guid id, UpdateIssueDto data);
    }
}