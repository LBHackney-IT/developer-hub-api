using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using DeveloperHubAPI.V1.Domain;
using Hackney.Core.DynamoDb.Converters;

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

        [DynamoDBProperty(Converter = typeof(DynamoDbObjectListConverter<Application>))]
        public List<Application> Applications { get; set; }

    }
}
