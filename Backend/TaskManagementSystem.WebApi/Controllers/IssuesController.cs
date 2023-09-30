using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TaskManagementSystem.Core;
using TaskManagementSystem.DataAccess;
using TaskManagementSystem.WebApi.Dto;

namespace TaskManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class IssuesController : ControllerBase
    {
        private readonly ILogger<IssuesController> _logger;
        private readonly IMapper _mapper;
        private readonly IssuesTree _tree;

        public IssuesController(ILogger<IssuesController> logger, IMapper mapper, IssuesTree tree)
        {
            _logger = logger;
            _mapper = mapper;
            _tree = tree;
        }

        /// <summary>
        /// Gets root issue titles by default or child issues by <paramref name="parentId"/>
        /// </summary>
        /// <param name="parentId">Parent issue id.</param>
        /// <response code="200">If successed.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IssueNodeShortDto>))]
        public async Task<IActionResult> GetRootIssuesAsync([FromQuery] Guid? parentId)
        {
            var nodes = await _tree.GetRootNodesListAsync();
            var response = _mapper.Map<IssueNodeShortDto[]>(nodes);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueNode))]
        public async Task<IActionResult> GetIssueAsync([FromRoute] Guid id)
        {
            var issue = await _tree.GetNodeAsync(id);
            if (issue == null)
                return NotFound();


            var response = new IssueNodeDto
            {
                Id = issue.Id,
                Title = issue.Title,
                IsLeaf = issue.IsLeaf,
                IsRoot = issue.IsRoot,
                Children = issue.Children.Select(x => new IssueNodeShortDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    IsLeaf = x.IsLeaf,
                    IsRoot = x.IsRoot
                }).ToArray()
            };

            return Ok(response);
        }

        [HttpGet("{id:guid}/children")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueNode[]))]
        public async Task<IActionResult> GetIssueChildrenAsync([FromRoute] Guid id)
        {
            var respone = await _tree.GetChildrenListAsync(id);
            return Ok(respone);
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
        public async Task<IActionResult> CreateIssueAsync([FromBody] CreateIssueRequest request)
        {
            var node = new IssueNode(request.Title, request.ParentId);
            var response = await _tree.CreateNodeAsync(node, request.ParentId);
            return CreatedAtRoute(new { Id = response }, response);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateIssueAsync(
            [FromRoute] Guid id, 
            [FromBody] UpdateIssueRequest request)
        {
            var node = new IssueNode(request.Title);
            var response = await _tree.UpdateNodeAsync(id, node);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteIssueAsync([FromRoute] Guid id)
        {
            await _tree.DeleteNodeAsync(id);
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
                return BadRequest("Попытка переместить пункт внутрь в самого себя");

            // return operation result ?
            await _tree.MoveNodeAsync(id, to);

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
            await _tree.MoveNodeAsync(id);
            return NoContent();
        }
    }
}
