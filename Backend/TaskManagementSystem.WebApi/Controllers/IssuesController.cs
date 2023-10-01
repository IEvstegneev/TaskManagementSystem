using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TaskManagementSystem.Core.Domain;
using TaskManagementSystem.Core.Dto;
using TaskManagementSystem.Core;
using TaskManagementSystem.DataAccess;

namespace TaskManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class IssuesController : ControllerBase
    {
        private readonly ILogger<IssuesController> _logger;
        private readonly IssuesService _issueService;

        public IssuesController(ILogger<IssuesController> logger, IssuesService issueService)
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

        [HttpGet("{id:guid}/children")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueNodeShortDto[]))]
        public async Task<IActionResult> GetIssueChildrenAsync([FromRoute] Guid id)
        {
            var respone = await _issueService.GetChildrenListAsync(id);
            return Ok(respone);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueNode))]
        public async Task<IActionResult> GetIssueAsync([FromRoute] Guid id)
        {
            var dto = await _issueService.GetIssueAsync(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }


        /// <summary>
        /// Create an issue.
        /// </summary>
        /// <param name="request" example='
        /// {
        ///  "title": "The main issue"
        /// }
        /// '>Create issue request dto.</param>
        /// <response code="201">If created.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
        public async Task<IActionResult> CreateIssueAsync([FromBody] CreateIssueDto request)
        {
            var response = await _issueService.CreateNodeAsync(request, request.ParentId);
            return CreatedAtRoute(new { Id = response }, response);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateIssueAsync(
            [FromRoute] Guid id, 
            [FromBody] UpdateIssueDto request)
        {
            var response = await _issueService.UpdateNodeAsync(id, request);
            if (response == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteIssueAsync([FromRoute] Guid id)
        {
            await _issueService.DeleteNodeAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Move an issue.
        /// </summary>
        /// <response code="200">If moved.</response>
        [HttpGet("{id:guid}/move")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MoveIssueAsync([FromRoute] Guid id, [FromQuery] Guid to)
        {
            if (id == to)
                return BadRequest("Moving issue id and destination issue id should be different.");

            // return operation result ?
            await _issueService.MoveNodeAsync(id, to);

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
            await _issueService.MoveNodeAsync(id);
            return NoContent();
        }


        [HttpGet("{id:guid}/change-status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueStatus))]
        public async Task<IActionResult> GetIssueAsync([FromRoute] Guid id, [FromQuery] IssueStatus status)
        {
            var result = await _issueService.ChangeStatusAsync(id, status);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
