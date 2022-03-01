using Microsoft.AspNetCore.Mvc;

namespace DeveloperHubAPI.V1.Boundary.Request
{

    public class CreateApplicationListItem
    {
        public string Name { get; set; }

        public string Link { get; set; }
    }
}
