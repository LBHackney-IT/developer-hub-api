using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using DeveloperHubAPI.Tests.V1.Helper;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.Infrastructure;
using DeveloperHubAPI.V1.Boundary.Request;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DeveloperHubAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class DynamoDbGatewayTests
    {
        private readonly Fixture _fixture = new Fixture();
        private Mock<IDynamoDBContext> _dynamoDb;
        private DynamoDbGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(_dynamoDb.Object);
        }

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = "1" }; 
        }

        [Test]
        public void GetDeveloperHubByIdReturnsNullIfEntityDoesntExist()
        {
            var query = ConstructQuery();
            var response = _classUnderTest.GetDeveloperHubById(query);

            response.Should().BeNull();
        }

        [Test]
        public void GetDeveloperHubByIdReturnsDeveloperHubIfItExists()
        {
            var entity = _fixture.Create<DeveloperHub>();
            var dbEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);
            var query = ConstructQuery();

            _dynamoDb.Setup(x => x.LoadAsync<DatabaseEntity>(entity.Id, default))
                     .ReturnsAsync(dbEntity);

            var response = _classUnderTest.GetDeveloperHubById(query);

            _dynamoDb.Verify(x => x.LoadAsync<DatabaseEntity>(query, default), Times.Once);

            query.Should().Be(response.Id);
        }
    }
}
