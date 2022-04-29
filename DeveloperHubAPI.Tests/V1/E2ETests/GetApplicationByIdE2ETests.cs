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
    public class GetApplicationByIdE2ETests : DynamoDbIntegrationTests<Startup>
    {
        private readonly Fixture _fixture = new Fixture();

        private async Task SetupTestData(DeveloperHubDb entity)
        {
            await DynamoDbContext.SaveAsync<DeveloperHubDb>(entity).ConfigureAwait(false);
            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync<DeveloperHubDb>(entity.Id).ConfigureAwait(false));
        }

        [Test]
        public async Task GetApplicationByIdReturns404()
        {
            // Arrange  
            var id = 123456789;
            var applicationId = Guid.NewGuid();
            var uri = new Uri($"api/v1/developerhubapi/{id}/{applicationId}", UriKind.Relative);

            // Act
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        [Test]
        public async Task GetApplicationByIdReturnsTheApplication()
        {
            // Arrange
            var application = _fixture.Create<Application>();
            var api = _fixture.Create<DevelopersHubApi>();
            api.Applications.Add(application);
            await SetupTestData(api.ToDatabase()).ConfigureAwait(false);
            var uri = new Uri($"api/v1/developerhubapi/{api.Id}/{application.Id}", UriKind.Relative);

            //Act
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<ApplicationResponse>(responseContent);

            apiEntity.Should().BeEquivalentTo(application);
        }
    }
}
