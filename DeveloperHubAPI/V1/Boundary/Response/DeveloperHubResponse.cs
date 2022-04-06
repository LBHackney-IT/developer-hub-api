using System.Collections.Generic;

namespace DeveloperHubAPI.V1.Boundary.Response
{
    public class DeveloperHubResponse
    {
        public string Id { get; set; }
        ///<example>"123"</example>

        public string ApiName { get; set; }
        ///<example>"Developer Hub"</example>

        public string Description { get; set; }
        ///<example>"Lorem ipsum dolor sit amet, consectetur adipiscing elit"</example>

        public string GithubLink { get; set; }
        ///<example>"https://github.com/"</example>

        public string SwaggerLink { get; set; }
        ///<example>"https://github.com/"</example>

        public string DevelopmentBaseURL { get; set; }
        /// <example>"https://github.com/"</example>

        public string StagingBaseURL { get; set; }
        ///<example>"https://github.com/"</example>

        public string ApiSpecificationLink { get; set; }
        ////<example>"https://github.com/"</example>

        public List<ApplicationResponse> Applications { get; set; }
    }
}
