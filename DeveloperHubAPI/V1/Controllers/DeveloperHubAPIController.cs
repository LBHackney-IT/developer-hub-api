using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Infrastructure;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using Hackney.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
        private readonly IGetApplicationByNameUseCase _getApplicationByNameUseCase;

        private readonly IDeleteApplicationByNameUseCase _deleteApplicationByNameUseCase;
        private readonly ICreateNewApplicationUseCase _createNewApplicationUseCase;

        public DeveloperHubAPIController(IGetDeveloperHubByIdUseCase getDeveloperHubByIdUseCase, IGetApplicationByNameUseCase getApplicationByNameUseCase, IDeleteApplicationByNameUseCase deleteApplicationByNameUseCase, ICreateNewApplicationUseCase createNewApplicationUseCase)
        {
            _getDeveloperHubByIdUseCase = getDeveloperHubByIdUseCase;
            _getApplicationByNameUseCase = getApplicationByNameUseCase;
            _deleteApplicationByNameUseCase = deleteApplicationByNameUseCase;
            _createNewApplicationUseCase = createNewApplicationUseCase;
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
        [Route("{id}/{applicationName}")]
        public async Task<IActionResult> GetApplication([FromRoute] ApplicationByNameRequest query)
        {
            var response = await _getApplicationByNameUseCase.Execute(query).ConfigureAwait(false);
            if (response == null) return NotFound(query.ApplicationName);
            return Ok(response);
        }

        /// <summary>
        /// deletes information about an application from the entity
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">No data found for the specified ID</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        [Route("{id}/{applicationName}")] //is this the correct route?
        public async Task<IActionResult> DeleteApplication([FromRoute] DeleteApplicationByNameRequest query)
        {
            var response = await _deleteApplicationByNameUseCase.Execute(query).ConfigureAwait(false);
            if (response == null) return NotFound(query.ApplicationName);

            return Ok(response);
        }

        /// adds information about an application that consumes the api
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="404">No data found for the specified ID</response>
        [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status404NotFound)]
        [HttpPost]
        [LogCall(LogLevel.Information)]
        [Route("{id}/{applicationName}")]
        public async Task<IActionResult> PostApplication([FromRoute] ApplicationByNameRequest pathParameters, [FromBody] CreateApplicationListItem bodyParameters)
        {
            var api = await _createNewApplicationUseCase.Execute(pathParameters, bodyParameters).ConfigureAwait(false);
            if (api == null) return NotFound(pathParameters.Id);
            return Created(new Uri($"api/v1/developerhubapi/{api.Id}/{api.Applications.LastOrDefault().Name}", UriKind.Relative), api); ;
        }
    }
}
