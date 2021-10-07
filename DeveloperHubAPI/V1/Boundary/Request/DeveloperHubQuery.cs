using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.Boundary.Request
{

    public class DeveloperHubQuery
    {
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }
}
