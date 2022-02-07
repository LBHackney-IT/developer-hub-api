using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;
using NUnit.Framework;

namespace DeveloperHubAPI.Tests.V1.Factories
{
    public class ResponseFactoryTest
    {
        [Test]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var dbEntity = new DeveloperHubDb();
            var response = dbEntity.ToDomain();

            dbEntity.Id.Should().Be(response.Id);
            dbEntity.ApiName.Should().Be(response.ApiName);
            dbEntity.Description.Should().Be(response.Description);
            dbEntity.GithubLink.Should().Be(response.GithubLink);
            dbEntity.SwaggerLink.Should().Be(response.SwaggerLink);
            dbEntity.DevelopmentBaseURL.Should().Be(response.DevelopmentBaseURL);
            dbEntity.StagingBaseURL.Should().Be(response.StagingBaseURL);
            dbEntity.ApiSpecificationLink.Should().Be(response.ApiSpecificationLink);
        }
    }
}
