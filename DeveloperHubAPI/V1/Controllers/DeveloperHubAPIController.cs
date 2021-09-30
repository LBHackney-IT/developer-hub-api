using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperHubAPI.V1.Controllers
{
    [DeveloperHubController]
    [Route("api/v1/developerhub")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class DeveloperHubAPIController : BaseController
    {
        private readonly IGetAllUseCase _getAllUseCase;
        private readonly IGetDeveloperHubByIdUseCase _getDeveloperHubByIdUseCase;
        public DeveloperHubAPIController(IGetAllUseCase getAllUseCase, IGetDeveloperHubByIdUseCase getDeveloperHubByIdUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _getDeveloperHubByIdUseCase = getDeveloperHubByIdUseCase;
        }

        /// <summary>
        /// Retrieve all data by person ID
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">No data found for the specified ID</response>
        [ProducesResponseType(typeof(DeveloperHubResponse), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("{Id}")]
        public IActionResult ViewDeveloperHub([FromQuery] DeveloperHubQuery query)
        {
            const response = _getDeveloperHubByIdUseCase.Execute(query)
            if (response == null) return NotFound();
            return Ok(response);
        }

         /// <summary>
        /// All data retrieved, in a list object
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid Query Parameter.</response>
        // [ProducesResponseType(typeof(DeveloperHubList), StatusCodes.Status200OK)]
        // [HttpGet]
        // public IActionResult DeveloperHubList()
        // {
        //     return Ok(_getAllUseCase.Execute());
        // }

    }
}
