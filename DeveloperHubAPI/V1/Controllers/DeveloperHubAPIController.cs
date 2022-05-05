using DeveloperHubAPI.V1.Authorization;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Infrastructure;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using Hackney.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.Controllers
{
    [ApiController]
    [Route("api/v1/developerhubapi")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class DeveloperHubAPIController : BaseController
    {
        private readonly IGetDeveloperHubByIdUseCase _getDeveloperHubByIdUseCase;
        private readonly IGetApplicationByIdUseCase _getApplicationByIdUseCase;

        private readonly IDeleteApplicationByIdUseCase _deleteApplicationByIdUseCase;

        private readonly IUpdateApplicationByIdUseCase _updateApplicationUseCase;
        public DeveloperHubAPIController(IGetDeveloperHubByIdUseCase getDeveloperHubByIdUseCase, IGetApplicationByIdUseCase getApplicationByIdUseCase, IDeleteApplicationByIdUseCase deleteApplicationByIdUseCase, IUpdateApplicationByIdUseCase updateApplicationUseCase)
        {
            _getDeveloperHubByIdUseCase = getDeveloperHubByIdUseCase;
            _getApplicationByIdUseCase = getApplicationByIdUseCase;
            _deleteApplicationByIdUseCase = deleteApplicationByIdUseCase;
            _updateApplicationUseCase = updateApplicationUseCase;
        }

        /// <summary>
        /// Retrieve all data by ID
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">No data found for the specified ID</response>
        [ProducesResponseType(typeof(DeveloperHubResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [LogCall(LogLevel.Information)]
        [Route("{Id}")]
        public async Task<IActionResult> ViewDeveloperHub([FromRoute] DeveloperHubQuery query)
        {
            var response = await _getDeveloperHubByIdUseCase.Execute(query).ConfigureAwait(false);
            if (response == null) return NotFound(query.Id);
            return Ok(response);
        }

        /// <summary>
        /// retrieves information about an application that consumes the api
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">No data found for the specified ID</response>
        [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [LogCall(LogLevel.Information)]
        [Route("{id}/application/{applicationId}")]
        public async Task<IActionResult> GetApplication([FromRoute] ApplicationByIdRequest query)
        {
            var response = await _getApplicationByIdUseCase.Execute(query).ConfigureAwait(false);
            if (response == null) return NotFound(query.ApplicationId);
            return Ok(response);
        }

        /// <summary>
        /// deletes information about an application from the entity
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">No data found for the specified ID</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        [AuthorizeByGroups("ALLOWED_GOOGLE_GROUPS")]
        [LogCall(LogLevel.Information)]
        [Route("{id}/application/{applicationId}")]
        public async Task<IActionResult> DeleteApplication([FromRoute] ApplicationByIdRequest query)
        {
            var response = await _deleteApplicationByIdUseCase.Execute(query).ConfigureAwait(false);
            if (response == null) return NotFound(query.ApplicationId);

            return Ok(response);
        }

        /// <summary>
        /// adds information about an application that consumes the api
        /// </summary>
        /// <response code="204">NoContent</response>
        /// <response code="404">No data found for the specified ID</response>
        [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status404NotFound)]
        [HttpPatch]
        [AuthorizeByGroups("ALLOWED_GOOGLE_GROUPS")]
        [LogCall(LogLevel.Information)]
        [Route("{id}/application/{applicationId}")]
        public async Task<IActionResult> PatchApplication([FromRoute] ApplicationByIdRequest pathParameters, [FromBody] UpdateApplicationListItem bodyParameters)
        {
            var api = await _updateApplicationUseCase.Execute(pathParameters, bodyParameters).ConfigureAwait(false);
            if (api == null) return NotFound(pathParameters.Id);
            return NoContent();
        }
    }
}
