using AutoFixture;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.Infrastructure;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Logging;
using Hackney.Core.Testing.Shared;
using DeveloperHubAPI.V1.Boundary.Request;

namespace DeveloperHubAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class DynamoDbGatewayTests : DynamoDbIntegrationTests<Startup>
    {
        private DynamoDbGateway _classUnderTest;
        private readonly Fixture _fixture = new Fixture();
        private Mock<ILogger<DynamoDbGateway>> _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<DynamoDbGateway>>();
            _classUnderTest = new DynamoDbGateway(DynamoDbContext, _logger.Object);
        }

        [Test]
        public async Task GetDeveloperHubByIdReturnsNullIfEntityDoesntExist()
        {
            var id = "random";
            var response = await _classUnderTest.GetDeveloperHubById(id).ConfigureAwait(false);

            response.Should().BeNull();
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for Developer Hub API ID: {id}", Times.Once());
        }

        [Test]
        public async Task GetDeveloperHubByIdReturnsTheEntityFromTheDatabase()
        {
            // Arrange
            var entity = _fixture.Build<DevelopersHubApi>()
                                    .With(x => x.ApiName, "DevelopersHubApi")
                                    .Create();
            var dbEntity = entity.ToDatabase();
            await InsertDataToDynamoDB(dbEntity).ConfigureAwait(false);

            // Act
            var result = await _classUnderTest.GetDeveloperHubById(entity.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeEquivalentTo(entity);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for Developer Hub API ID: {entity.Id}", Times.Once());
        }

        public async Task SaveDeveloperHubSuccessfullySavesTheEntity()
        {
            // Arrange
            var entity = _fixture.Build<DevelopersHubApi>()
                                    .With(x => x.ApiName, "DevelopersHubApi")
                                    .Create();

            // Act
            await _classUnderTest.SaveDeveloperHub(entity).ConfigureAwait(false);


            // Assert
            var result = await DynamoDbContext.LoadAsync<DeveloperHubDb>(entity.Id).ConfigureAwait(false);
            result.Should().BeEquivalentTo(entity);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.SaveAsync for Developer Hub API: {entity.Id}", Times.Once());
            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync<DeveloperHubDb>(entity.Id).ConfigureAwait(false));
        }

        private async Task InsertDataToDynamoDB(DeveloperHubDb entity)
        {
            await DynamoDbContext.SaveAsync<DeveloperHubDb>(entity).ConfigureAwait(false);
            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync<DeveloperHubDb>(entity.Id).ConfigureAwait(false));
        }

    }
}
