using Microsoft.AspNetCore.Mvc;

namespace DeveloperHubAPI.V1.Boundary.Request
{

    public class DeleteApplicationByNameRequest
    {
        [FromRoute(Name = "id")]
        public string Id { get; set; }

        [FromRoute(Name = "applicationName")]
        public string ApplicationName { get; set; }
    }
}