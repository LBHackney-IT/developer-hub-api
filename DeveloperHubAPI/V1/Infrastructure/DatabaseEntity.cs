using Amazon.DynamoDBv2.DataModel;

namespace DeveloperHubAPI.V1.Infrastructure
{

    [DynamoDBTable("DevelopersHubApi", LowerCamelCaseProperties = true)]
    public class DatabaseEntity
    {

        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string ApiName { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public string GithubLink { get; set; }

        [DynamoDBProperty]
        public string SwaggerLink { get; set; }

        [DynamoDBProperty]
        public string DevelopmentBaseURL { get; set; }

        [DynamoDBProperty]
        public string StagingBaseURL { get; set; }

        [DynamoDBProperty]
        public string ApiSpecificationLink { get; set; }
        
        [DynamoDBProperty]
        public List<Application> Applications { get; set; }

    }
}
