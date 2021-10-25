using System;

namespace DeveloperHubAPI.V1.Domain
{
    public class DevelopersHubApi
    {
        public string Id { get; set; }

        public string ApiName { get; set; }

        public string Description { get; set; }

        public string GithubLink { get; set; }

        public string SwaggerLink { get; set; }

        public string DevelopmentBaseURL { get; set; }

        public string StagingBaseURL { get; set; }

        public string ApiSpecificationLink { get; set; }
    }
}
