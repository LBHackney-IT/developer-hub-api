using AutoFixture;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Infrastructure;

namespace DeveloperHubAPI.Tests.V1.Helper
{
    public static class DatabaseEntityHelper
    {
        public static DatabaseEntity CreateDatabaseEntity()
        {
            var entity = new Fixture().Create<DeveloperHub>();

            return CreateDatabaseEntityFrom(entity);
        }

        public static DatabaseEntity CreateDatabaseEntityFrom(DeveloperHub domain)
        {
            return new DatabaseEntity
            {
                Id = domain.Id,
                ApiName = domain.ApiName,
                Description = domain.Description,
                GithubLink = domain.GithubLink,
                SwaggerLink = domain.SwaggerLink,
                DevelopmentBaseURL = domain.DevelopmentBaseURL,
                StagingBaseURL = domain.StagingBaseURL,
                ApiSpecificationLink = domain.ApiSpecificationLink
            };
        }
    }
}
