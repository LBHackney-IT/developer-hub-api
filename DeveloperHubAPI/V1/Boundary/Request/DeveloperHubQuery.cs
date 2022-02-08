using Microsoft.AspNetCore.Mvc;

namespace DeveloperHubAPI.V1.Boundary.Request
{

    public class DeveloperHubQuery
    {
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }
}
