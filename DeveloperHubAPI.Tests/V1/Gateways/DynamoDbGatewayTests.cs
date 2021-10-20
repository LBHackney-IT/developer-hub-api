using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeveloperHubAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class DynamoDbGatewayTests : DynamoDbIntegrationTests<Startup>
    {
        private IDynamoDBContext _dynamoDb;
        private DynamoDbGateway _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp(DynamoDbIntegrationTests<Startup> dbTestFixture)
        {
            _dynamoDb = dbTestFixture.DynamoDbContext;
            _classUnderTest = new DynamoDbGateway(_dynamoDb);
        }

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = "1" };
        }

        [Test]
        public async Task GetDeveloperHubByIdReturnsNullIfEntityDoesntExist()
        {
            var query = ConstructQuery();
            var response = await _classUnderTest.GetDeveloperHubById(query).ConfigureAwait(false); // amend test so query type will return null

            response.Should().BeNull();
        }

        [Test]
        public void GetDeveloperHubByIdReturnsDeveloperHubIfItExists()
        {
            var entity = _fixture.Create<DeveloperHub>();
            var dbEntity = entity.ToDatabase();
            var query = ConstructQuery();

            var response = _classUnderTest.GetDeveloperHubById(query);

            query.Should().Be(response.Id);
        }

        [Test]
        public async Task VerifiesGatewayMethodsAddToDB()
        {   
            // Arrange
            var query = ConstructQuery();
            var entity = _fixture.Build<DatabaseEntity>()
                                    .With(x => x.ApiName, "DeveloperHub").Create();
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
