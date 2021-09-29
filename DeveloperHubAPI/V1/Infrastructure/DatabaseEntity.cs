using Amazon.DynamoDBv2.DataModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeveloperHubAPI.V1.Infrastructure
{
    
    [DynamoDBTable("DeveloperHub", LowerCamelCaseProperties = true)]
    public class DatabaseEntity
    {
        
        [DynamoDBHashKey]
        public int Id { get; set; }

        [DynamoDBProperty]
        public string  ApiName { get; set; }

        [DynamoDBProperty]
        public string  Description { get; set; }

        [DynamoDBProperty]
        public string  GithubLink { get; set; }

        [DynamoDBProperty]
        public string  SwaggerLink { get; set; }

        [DynamoDBProperty]
        public string  DevelopmentBaseURL { get; set; }

        [DynamoDBProperty]
        public string  StagingBaseURL { get; set; }

        [DynamoDBProperty]
        public string  ApiSpecificationLink { get; set; }

    }
}
