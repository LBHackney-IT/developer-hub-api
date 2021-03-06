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

        [Test]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var databaseEntity = _fixture.Create<DeveloperHubDb>();
            var entity = databaseEntity.ToDomain();

            databaseEntity.Id.Should().Be(entity.Id);
            databaseEntity.ApiName.Should().Be(entity.ApiName);
            databaseEntity.Description.Should().Be(entity.Description);
            databaseEntity.GithubLink.Should().Be(entity.GithubLink);
            databaseEntity.SwaggerLink.Should().Be(entity.SwaggerLink);
            databaseEntity.DevelopmentBaseURL.Should().Be(entity.DevelopmentBaseURL);
            databaseEntity.StagingBaseURL.Should().Be(entity.StagingBaseURL);
            databaseEntity.ApiSpecificationLink.Should().Be(entity.ApiSpecificationLink);
            databaseEntity.Applications.Should().BeEquivalentTo(entity.Applications);
        }

        [Test]
        public void CanMapDomainToADatabaseObject()
        {
            var entity = _fixture.Create<DevelopersHubApi>();
            var databaseEntity = entity.ToDatabase();

            entity.Id.Should().Be(databaseEntity.Id);
            entity.ApiName.Should().Be(databaseEntity.ApiName);
            entity.Description.Should().Be(databaseEntity.Description);
            entity.GithubLink.Should().Be(databaseEntity.GithubLink);
            entity.SwaggerLink.Should().Be(databaseEntity.SwaggerLink);
            entity.DevelopmentBaseURL.Should().Be(databaseEntity.DevelopmentBaseURL);
            entity.StagingBaseURL.Should().Be(databaseEntity.StagingBaseURL);
            entity.ApiSpecificationLink.Should().Be(databaseEntity.ApiSpecificationLink);
            entity.Applications.Should().BeEquivalentTo(databaseEntity.Applications);
        }
    }
}
