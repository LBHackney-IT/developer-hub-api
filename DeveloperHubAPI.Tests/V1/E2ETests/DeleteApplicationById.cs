using AutoFixture;
using DeveloperHubAPI.Tests.V1.E2ETests.Constants;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperHubAPI.Tests.V1.E2ETests
{
    [TestFixture]
    public class DeleteApplicationById : DynamoDbIntegrationTests<Startup>
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

        public async Task DeleteApplicationByNameReturns404NotFound()
        {
            // Arrange
            var id = 987654321;
            var applicationId = Guid.NewGuid();
            var uri = new Uri($"api/v1/developerhubapi/{id}/application/{applicationId}", UriKind.Relative);
            var bodyParameters = _fixture.Create<ApplicationByIdRequest>();

            // Act
            var message = new HttpRequestMessage(HttpMethod.Delete, uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(bodyParameters), Encoding.UTF8, "application/json");
            message.Headers.Add("Authorization", TestToken.Value);
            var httpResponse = await Client.SendAsync(message).ConfigureAwait(false);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            message.Dispose();
        }

        [Test]
        public async Task DeleteApplicationByNameDeletesTheApplication()
        {
            // Arrange
            var application = _fixture.Create<Application>();
            var api = _fixture.Create<DevelopersHubApi>();
            api.Applications.Add(application);
            await SetupTestData(api.ToDatabase()).ConfigureAwait(false);
            var uri = new Uri($"api/v1/developerhubapi/{api.Id}/application/{application.Id}", UriKind.Relative);
            var bodyParameters = _fixture.Create<ApplicationByIdRequest>();

            // Act
            var message = new HttpRequestMessage(HttpMethod.Delete, uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(bodyParameters), Encoding.UTF8, "application/json");
            message.Headers.Add("Authorization", TestToken.Value);
            var httpResponse = await Client.SendAsync(message).ConfigureAwait(false);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            message.Dispose();
        }

        public async Task DeleteApplicationByNameReturns401Unauthorized()
        {
            // Arrange
            var id = 987654321;
            var applicationId = Guid.NewGuid();
            var uri = new Uri($"api/v1/developerhubapi/{id}/application/{applicationId}", UriKind.Relative);
            var bodyParameters = _fixture.Create<ApplicationByIdRequest>();

            // Act
            var message = new HttpRequestMessage(HttpMethod.Delete, uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(bodyParameters), Encoding.UTF8, "application/json");
            var httpResponse = await Client.SendAsync(message).ConfigureAwait(false);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            message.Dispose();
        }

    }
}
