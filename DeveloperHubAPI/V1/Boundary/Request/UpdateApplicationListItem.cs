using Microsoft.AspNetCore.Mvc;
using System;

namespace DeveloperHubAPI.V1.Boundary.Request
{

    public class UpdateApplicationListItem
    {
        public string Name { get; set; }

        public string Link { get; set; }
    }
}
