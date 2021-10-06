using AutoFixture;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperHubAPI.Tests.V1.Factories
{
    [TestFixture]
    public class EntityFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();

        //TODO: add assertions for all the fields being mapped in `EntityFactory.ToDomain()`. Also be sure to add test cases for
        // any edge cases that might exist.
        [Test]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var databaseEntity = _fixture.Create<DatabaseEntity>();
            var entity = databaseEntity.ToDomain();

            databaseEntity.Id.Should().Be(entity.Id);
            databaseEntity.ApiName.Should().Be(entity.ApiName);
            databaseEntity.Description.Should().Be(entity.Description);
            databaseEntity.GithubLink.Should().Be(entity.GithubLink);
            databaseEntity.SwaggerLink.Should().Be(entity.SwaggerLink);
            databaseEntity.DevelopmentBaseURL.Should().Be(entity.DevelopmentBaseURL);
            databaseEntity.StagingBaseURL.Should().Be(entity.StagingBaseURL);
            databaseEntity.ApiSpecificationLink.Should().Be(entity.ApiSpecificationLink);
        }

        //TODO: add assertions for all the fields being mapped in `EntityFactory.ToDatabase()`. Also be sure to add test cases for
        // any edge cases that might exist.
        [Test]
        public void CanMapDeveloperHubToADatabaseObject()
        {
            var entity = _fixture.Create<DeveloperHub>();
            var databaseEntity = entity.ToDatabase();

            entity.Id.Should().Be(databaseEntity.Id);
            entity.ApiName.Should().Be(databaseEntity.ApiName);
            entity.Description.Should().Be(databaseEntity.Description);
            entity.GithubLink.Should().Be(databaseEntity.GithubLink);
            entity.SwaggerLink.Should().Be(databaseEntity.SwaggerLink);
            entity.DevelopmentBaseURL.Should().Be(databaseEntity.DevelopmentBaseURL);
            entity.StagingBaseURL.Should().Be(databaseEntity.StagingBaseURL);
            entity.ApiSpecificationLink.Should().Be(databaseEntity.ApiSpecificationLink);
        }
    }
}
