using AutoFixture;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Infrastructure;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeveloperHubAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class DynamoDbGatewayTests : DynamoDbIntegrationTests<Startup>
    {
        private DynamoDbGateway _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _classUnderTest = new DynamoDbGateway(DynamoDbContext);
        }

        private static DeveloperHubQuery ConstructQuery(string id)
        {
            return new DeveloperHubQuery() { Id = id };
        }

        [Test]
        public async Task GetDeveloperHubByIdReturnsNullIfEntityDoesntExist()
        {
            var query = ConstructQuery("1");
            var response = await _classUnderTest.GetDeveloperHubById(query).ConfigureAwait(false);

            response.Should().BeNull();
        }

        [Test]
        public async Task VerifiesGatewayMethodsAddToDB()
        {
            // Arrange
            var entity = _fixture.Build<DevelopersHubApi>()
                                    .With(x => x.ApiName, "DevelopersHubApi")
                                    .Create();
            var dbEntity = entity.ToDatabase();
            await InsertDataToDynamoDB(dbEntity).ConfigureAwait(false);
            var query = ConstructQuery(entity.Id);

            // Act
            var result = await _classUnderTest.GetDeveloperHubById(query).ConfigureAwait(false);

            // Assert
            result.Should().BeEquivalentTo(entity);
        }

        private async Task InsertDataToDynamoDB(DatabaseEntity entity)
        {
            await DynamoDbContext.SaveAsync<DatabaseEntity>(entity).ConfigureAwait(false);
            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync(entity).ConfigureAwait(false));
        }

    }
}
