using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.Controllers
{
    [ApiController]
    [Route("api/v1/developerhub")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class DeveloperHubAPIController : BaseController
    {
        private readonly IGetDeveloperHubByIdUseCase _getDeveloperHubByIdUseCase;
        public DeveloperHubAPIController( IGetDeveloperHubByIdUseCase getDeveloperHubByIdUseCase)
        {
            _getDeveloperHubByIdUseCase = getDeveloperHubByIdUseCase;
        }

        /// <summary>
        /// Retrieve all data by ID
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">No data found for the specified ID</response>
        [ProducesResponseType(typeof(DeveloperHubResponse), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> ViewDeveloperHub([FromQuery] DeveloperHubQuery query)
        {
            var response = await _getDeveloperHubByIdUseCase.Execute(query).ConfigureAwait(false);
            if (response == null) return NotFound(query.Id);
            return Ok(response);
        }

    }
}
