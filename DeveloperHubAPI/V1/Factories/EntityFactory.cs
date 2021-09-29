using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Infrastructure;

namespace DeveloperHubAPI.V1.Factories
{
    public static class EntityFactory
    {
        public static DeveloperHub ToDomain(this DatabaseEntity databaseEntity)
        {
            return new DeveloperHub
            {
                Id = databaseEntity.Id,
                ApiName = databaseEntity.ApiName,
                Description = databaseEntity.Description,
                GithubLink = databaseEntity.GithubLink,
                SwaggerLink = databaseEntity.SwaggerLink,
                DevelopmentBaseURL = databaseEntity.DevelopmentBaseURL,
                StagingBaseURL = databaseEntity.StagingBaseURL,
                ApiSpecificationLink = databaseEntity.ApiSpecificationLink
            };
        }

        public static DatabaseEntity ToDatabase(this DeveloperHub developerHub)
        {

            return new DatabaseEntity
            {
                Id = developerHub.Id,
                ApiName = developerHub.ApiName,
                Description = developerHub.Description,
                GithubLink = developerHub.GithubLink,
                SwaggerLink = developerHub.SwaggerLink,
                DevelopmentBaseURL = developerHub.DevelopmentBaseURL,
                StagingBaseURL = developerHub.StagingBaseURL,
                ApiSpecificationLink = developerHub.ApiSpecificationLink
            };
        }
    }
}
