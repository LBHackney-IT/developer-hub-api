using AutoFixture;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Infrastructure;

namespace DeveloperHubAPI.Tests.V1.Helper
{
    public static class DatabaseEntityHelper
    {
        public static DatabaseEntity CreateDatabaseEntity()
        {
            var entity = new Fixture().Create<DatabaseEntity>();

            return CreateDatabaseEntityFrom(entity);
        }

        public static DatabaseEntity CreateDatabaseEntityFrom(DatabaseEntity databaseEntity)
        {
            return new DatabaseEntity
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
    }
}
