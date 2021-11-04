using AutoFixture;
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
    public class GetByIdE2ETests : DynamoDbIntegrationTests<Startup>
    {
        private readonly Fixture _fixture = new Fixture();

        /// <summary>
        /// Method to construct a test entity that can be used in a test
        /// </summary>
        /// <param name="databaseEntity"></param>
        /// <returns></returns>
        private DatabaseEntity ConstructTestEntity()
        {
            var entity = _fixture.Create<DatabaseEntity>();
            return entity;
        }

        /// <summary>
        /// Method to add an entity instance to the database so that it can be used in a test.
        /// Also adds the corresponding action to remove the upserted data from the database when the test is done.
        /// </summary>
        /// <param name="databaseEntity"></param>
        /// <returns></returns>
        private async Task SetupTestData(DatabaseEntity entity)
        {
            await DynamoDbContext.SaveAsync<DatabaseEntity>(entity).ConfigureAwait(false);
            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync<DatabaseEntity>(entity.Id).ConfigureAwait(false));
        }

        [Test]
        public async Task GetEntityByIdNotFoundReturns404()
        {
            int id = 123456789;
            var uri = new Uri($"api/v1/developerhubapi/{id}", UriKind.Relative);
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetDeveloperHubApiBydIdFoundReturnsResponse()
        {
            var entity = ConstructTestEntity();
            await SetupTestData(entity).ConfigureAwait(false);

            var uri = new Uri($"api/v1/developerhubapi/{entity.Id}", UriKind.Relative);
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<DatabaseEntity>(responseContent);

            apiEntity.Should().BeEquivalentTo(entity);
        }
    }
}
