using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Infrastructure;

namespace DeveloperHubAPI.V1.Factories
{
    public static class EntityFactory
    {
        public static DevelopersHubApi ToDomain(this DatabaseEntity databaseEntity)
        {
            return new DevelopersHubApi
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

        public static DatabaseEntity ToDatabase(this DevelopersHubApi developerHubApi)
        {

            return new DatabaseEntity
            {
                Id = developerHubApi.Id,
                ApiName = developerHubApi.ApiName,
                Description = developerHubApi.Description,
                GithubLink = developerHubApi.GithubLink,
                SwaggerLink = developerHubApi.SwaggerLink,
                DevelopmentBaseURL = developerHubApi.DevelopmentBaseURL,
                StagingBaseURL = developerHubApi.StagingBaseURL,
                ApiSpecificationLink = developerHubApi.ApiSpecificationLink
            };
        }
    }
}
