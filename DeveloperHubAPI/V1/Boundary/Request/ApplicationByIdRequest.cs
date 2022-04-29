using Microsoft.AspNetCore.Mvc;
using System;

namespace DeveloperHubAPI.V1.Boundary.Request
{

    public class ApplicationByIdRequest
    {
        [FromRoute(Name = "id")]
        public string Id { get; set; }

        [FromRoute(Name = "applicationId")]
        public Guid ApplicationId { get; set; }
    }
}
