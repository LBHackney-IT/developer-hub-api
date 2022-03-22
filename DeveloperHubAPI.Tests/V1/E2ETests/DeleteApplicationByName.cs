using AutoFixture;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DeveloperHubAPI.Tests.V1.E2ETests
{
    [TestFixture]
    public class DeleteApplicationByName : DynamoDbIntegrationTests<Startup>
    {
        private readonly Fixture _fixture = new Fixture();

        private DeveloperHubDb ConstructTestEntity()
        {
            var entity = _fixture.Create<DeveloperHubDb>();
            return entity;
        }

        private async Task SetupTestData(DeveloperHubDb entity)
        {
            await DynamoDbContext.SaveAsync<DeveloperHubDb>(entity).ConfigureAwait(false);
            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync<DeveloperHubDb>(entity.Id).ConfigureAwait(false));
        }

        [Test]

        public async Task DeleteApplicationByNameReturns404()
        {
            var id = 987654321;
            var applicationName = "random";
            var uri = new Uri($"api/v1/developerhubapi/{id}/{applicationName}", UriKind.Relative);

            var response = await Client.DeleteAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Test]
        public async Task DeleteApplicationByNameDeletesTheApplication()
        {
            // Arrange
            var application = _fixture.Create<Application>();
            var api = _fixture.Create<DevelopersHubApi>();
            api.Applications.Add(application);
            await SetupTestData(api.ToDatabase()).ConfigureAwait(false);
            var uri = new Uri($"api/v1/developerhubapi/{api.Id}/{application.Name}", UriKind.Relative);

            //Act
            var response = await Client.DeleteAsync(uri).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
