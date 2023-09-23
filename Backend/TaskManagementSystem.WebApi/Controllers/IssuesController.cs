using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Core;
using TaskManagementSystem.WebApi.Dto;

namespace TaskManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly ILogger<IssuesController> _logger;
        private readonly IRepository<IssueNode> _repository;

        public IssuesController(ILogger<IssuesController> logger, IRepository<IssueNode> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IssueNode>))]
        public async Task<IActionResult> GetIssuesAsync()
        {
            var response = await _repository.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueNode))]
        public async Task<IActionResult> GetIssueAsync([FromRoute] Guid id)
        {
            var respone = await _repository.GetByIdAsync(id);
            return Ok(respone);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
        public async Task<IActionResult> CreateIssueAsync(CreateIssueRequest request)
        {
            var respones = await _repository.CreateAsync(new IssueNode(request.Name));
            return CreatedAtRoute(new { Id = respones }, respones);
        }
    }
}
