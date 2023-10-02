using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Abstractions;
using TaskManagementSystem.Core.Domain;
using TaskManagementSystem.Core.Dto;
using TaskManagementSystem.DataAccess;

namespace TaskManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly ILogger<IssuesController> _logger;
        private readonly IIssuesService _issueService;

        public IssuesController(ILogger<IssuesController> logger, IIssuesService issueService)
        {
            _logger = logger;
            _issueService = issueService;
        }

        /// <summary>
        /// Gets root issue titles by default or child issues by <paramref name="parentId"/>
        /// </summary>
        /// <param name="parentId">Parent issue id.</param>
        /// <response code="200">If successed.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueNodeShortDto[]))]
        public async Task<IActionResult> GetRootIssuesAsync([FromQuery] Guid? parentId)
        {
            var response = await _issueService.GetRootIssuesListAsync();
            return Ok(response);
        }

        /// <summary>
        /// Gets child issues list.
        /// </summary>
        /// <param name="id">Issue id.</param>
        [HttpGet("{id:guid}/children")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueNodeShortDto[]))]
        public async Task<IActionResult> GetIssueChildrenAsync([FromRoute] Guid id)
        {
            var respone = await _issueService.GetChildrenListAsync(id);
            return Ok(respone);
        }

        /// <summary>
        /// Gets issue detail data.
        /// </summary>
        /// <param name="id">Issue id.</param>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueNodeDto))]
        public async Task<IActionResult> GetIssueAsync([FromRoute] Guid id)
        {
            var result = await _issueService.GetIssueAsync(id);
            if (result.IsFailed)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }


        /// <summary>
        /// Create an issue.
        /// </summary>
        /// <param name="request" example='
        /// {
        ///  "title": "The great task."
        /// }
        /// '>Create issue request dto.</param>
        /// <response code="201">If created.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
        public async Task<IActionResult> CreateIssueAsync([FromBody] CreateIssueDto request)
        {
            var result = await _issueService.CreateNodeAsync(request, request.ParentId);
            if (result.IsFailed)
                return BadRequest(result.ErrorMessage);

            return CreatedAtRoute(new { Id = result.Value }, result.Value);
        }

        /// <summary>
        /// Updates the necessary data.
        /// </summary>
        /// <param name="id">Issue id.</param>
        /// <param name="request">New data.</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateIssueAsync(
            [FromRoute] Guid id, 
            [FromBody] UpdateIssueDto request)
        {
            var result = await _issueService.UpdateNodeAsync(id, request);
            if (result.IsFailed)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        /// <summary>
        /// Delete an issue.
        /// </summary>
        /// <param name="id">Issue id.</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteIssueAsync([FromRoute] Guid id)
        {
            var result = await _issueService.DeleteNodeAsync(id);
            if (result.IsFailed)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        /// <summary>
        /// Move an issue into other issue.
        /// </summary>
        /// <response code="200">If moved.</response>
        [HttpGet("{id:guid}/move")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MoveIssueAsync(
            [FromRoute] Guid id, 
            [FromQuery] Guid to)
        {
            if (id == to)
                return BadRequest(Errors.Moving.IdShouldBeDifferent);

            var result = await _issueService.MoveNodeAsync(id, to);
            if (result.IsFailed)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        /// <summary>
        /// Move an issue to root.
        /// </summary>
        /// <response code="200">If moved.</response>
        [HttpGet("{id:guid}/move-to-root")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> MoveToRootAsync([FromRoute] Guid id)
        {
            var result = await _issueService.MoveNodeToRootAsync(id);
            if (result.IsFailed)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        /// <summary>
        /// Change issue status.
        /// </summary>
        /// <param name="id">Target issue id.</param>
        /// <param name="status">The new status.</param>
        [HttpGet("{id:guid}/change-status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangeIssueStatusAsync(
            [FromRoute] Guid id, 
            [FromQuery] IssueStatus status)
        {
            var result = await _issueService.ChangeStatusAsync(id, status);
            if (result.IsFailed)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }
    }
}
