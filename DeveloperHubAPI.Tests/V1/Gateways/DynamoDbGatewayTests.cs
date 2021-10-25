using AutoFixture;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Infrastructure;
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

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = "1" };
        }

        [Test]
        public async Task GetDeveloperHubByIdReturnsNullIfEntityDoesntExist()
        {
            var query = ConstructQuery();
            var response = await _classUnderTest.GetDeveloperHubById(query).ConfigureAwait(false);

            response.Should().BeNull();
        }

        [Test]
        public async Task VerifiesGatewayMethodsAddToDB()
        {
            // Arrange
            var query = ConstructQuery();
            var entity = _fixture.Build<DatabaseEntity>()
                                    .With(x => x.Id, query.Id)
                                    .With(x => x.ApiName, "DevelopersHubApi")
                                    .Create();
            InsertDataToDynamoDB(entity);

            // Act
            var result = await _classUnderTest.GetDeveloperHubById(query).ConfigureAwait(false);

            // Assert
            result.Should().BeEquivalentTo(entity);
        }

        private void InsertDataToDynamoDB(DatabaseEntity entity)
        {
            DynamoDbContext.SaveAsync<DatabaseEntity>(entity).ConfigureAwait(false);
            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync(entity).ConfigureAwait(false));
        }

    }
}
