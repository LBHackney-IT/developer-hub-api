namespace DeveloperHubAPI.V1.Boundary.Response
{
    public class DevelopmentHubResponse
    {
        public int Id { get; set; }
        ///<example>123</example>
        
        public string  ApiName { get; set; }
        ///<example>Developer Hub</example>

        public string  Description { get; set; }
        ///<example>Lorem ipsum dolor sit amet, consectetur adipiscing elit</example>

        public string GithubLink { get; set; }
        ///<example>https://github.com/</example>

        public string  SwaggerLink { get; set; }
        ///<example>https://github.com/</example>

        public string DevelopmentLinks { get; set;}
        /// <example>https://github.com/</example>

         public string StagingLinks { get; set; }
        /// <example>https://github.com/</example>

        public string  StagingBaseURL { get; set; }
        ///<example>https://github.com/</example>

        public string  ApiSpecificationLink { get; set; }
        ///<example>https://github.com/</example>
    }
}
