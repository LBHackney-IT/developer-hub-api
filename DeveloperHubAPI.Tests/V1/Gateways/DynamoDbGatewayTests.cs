using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.Infrastructure;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Factories;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeveloperHubAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class DynamoDbGatewayTests : IDisposable
    {
        private IDynamoDBContext _dynamoDb;
        private DynamoDbGateway _classUnderTest;
        private readonly List<Action> _cleanup = new List<Action>();
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _dynamoDb = new IDynamoDBContext();
            _classUnderTest = new DynamoDbGateway(_dynamoDb);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var action in _cleanup)
                    action();

                _disposed = true;
            }
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
            var dbEntity = entity.ToDatabase();
            var query = ConstructQuery();

            _dynamoDb.Setup(x => x.LoadAsync<DatabaseEntity>(query, default))
                     .ReturnsAsync(dbEntity); 

            var response = _classUnderTest.GetDeveloperHubById(query);

            _dynamoDb.Verify(x => x.LoadAsync<DatabaseEntity>(query, default), Times.Once);

            query.Should().Be(response.Id);
        }

        [Test]
        public async Task GetDeveloperHubByIdReturnsResponseIfItExists()
        {
            // Arrange
            var entity = _fixture.Create<DeveloperHub>();
            var dbEntity = entity.ToDatabase();

            await _dynamoDb.SaveAsync(dbEntity).ConfigureAwait(false);
            _cleanup.Add(async () => await _dynamoDb.DeleteAsync(dbEntity).ConfigureAwait(false));

            // Act
            var query = ConstructQuery();
            var response = await _classUnderTest.GetDeveloperHubById(query).ConfigureAwait(false);

            // Assert
            response.ApiName.Should().Be(entity.ApiName);
            response.Description.Should().Be(entity.Description);
            response.GithubLink.Should().Be(entity.GithubLink);
            response.SwaggerLink.Should().Be(entity.SwaggerLink);
            response.DevelopmentBaseURL.Should().Be(entity.DevelopmentBaseURL);
            response.StagingBaseURL.Should().Be(entity.StagingBaseURL);
            response.ApiSpecificationLink.Should().Be(entity.ApiSpecificationLink);
        }

        // [Test]
        // public void GetDeveloperHubByIdExceptionThrow()
        // {
        //      // Arrange
        //     var mockDynamoDb = new DynamoDBContext();
        //     _classUnderTest = new DynamoDbGateway(mockDynamoDb);
        //     var id = Guid.NewGuid();
        //     var query = ConstructQuery();
        //     var exception = new ApplicationException("Test exception");
        //     mockDynamoDb.Setup(x => x.LoadAsync<DatabaseEntity>(id, default))
        //                 .ThrowsAsync(exception);

        //     // Act
        //     Func<Task<DeveloperHub>> func = async () => await _classUnderTest.GetDeveloperHubById(query).ConfigureAwait(false);

        //     // Assert
        //     func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        //     mockDynamoDb.Verify(x => x.LoadAsync<DatabaseEntity>(query, default), Times.Once);
        // }
    }
}
