using AutoFixture;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperHubAPI.Tests.V1.E2ETests
{
    [TestFixture]
    public class UpdateApplicationE2ETests : DynamoDbIntegrationTests<Startup>
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
        public async Task UpdateApplicationReturns404()
        {
            // Arrange  
            var id = 123456789;
            var applicationName = "random";
            var uri = new Uri($"api/v1/developerhubapi/{id}/{applicationName}", UriKind.Relative);
            var bodyParameters = _fixture.Create<UpdateApplicationListItem>();

            // Act
            var message = new HttpRequestMessage(HttpMethod.Patch, uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(bodyParameters), Encoding.UTF8, "application/json");
            var response = await Client.SendAsync(message).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            message.Dispose();
        }

        [Test]
        public async Task UpdateApplicationReturns204()
        {
            // Arrange  
            var bodyParameters = _fixture.Create<UpdateApplicationListItem>();
            var api = _fixture.Create<DevelopersHubApi>();
            var uri = new Uri($"api/v1/developerhubapi/{api.Id}/{bodyParameters.Name}", UriKind.Relative);
            await SetupTestData(api.ToDatabase()).ConfigureAwait(false);
            // Act
            var message = new HttpRequestMessage(HttpMethod.Patch, uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(bodyParameters), Encoding.UTF8, "application/json");
            var response = await Client.SendAsync(message).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var updatedApi = await DynamoDbContext.LoadAsync<DeveloperHubDb>(api.Id).ConfigureAwait(false);
            updatedApi.Applications.LastOrDefault().Name.Should().Be(bodyParameters.Name);
            updatedApi.Applications.LastOrDefault().Link.Should().Be(bodyParameters.Link);
            message.Dispose();
        }
    }
}
