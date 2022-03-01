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

            response.StatusCode.Should().Be(HttpStatusCode.OK);
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
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<ApplicationResponse>(responseContent);

            apiEntity.Should().BeEquivalentTo(application);
        }

        //    [Fact]
        //    public async Task CreateAndDeleteChargeReturns204()
        //    {
        //        var charge = ConstructCharge();

        //        var uri = new Uri($"api/v1/charges", UriKind.Relative);

        //        string body = JsonConvert.SerializeObject(charge);

        //        using StringContent stringContent = new StringContent(body);

        //        stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //        using var response = await Client.PostAsync(uri, stringContent).ConfigureAwait(false);

        //        response.StatusCode.Should().Be(HttpStatusCode.Created);

        //        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //        var apiEntity = JsonConvert.DeserializeObject<ChargeResponse>(responseContent);

        //        apiEntity.Should().NotBeNull();

        //        apiEntity.Should().BeEquivalentTo(charge, options => options.Excluding(a => a.Id));

        //        var deleteUri = new Uri($"api/v1/charges/{apiEntity.Id}?targetId={apiEntity.TargetId}", UriKind.Relative);

        //        using var deleteResponse = await Client.DeleteAsync(deleteUri).ConfigureAwait(false);

        //        deleteResponse.StatusCode.Should().Be(204);
        //    }
    }
}
