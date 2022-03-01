using System;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.UseCase;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Boundary.Request;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using AutoFixture;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.Tests.V1.Helper;

namespace DeveloperHubAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class CreateNewApplicationUseCaseTests : LogCallTestContext
    {
        private Mock<IDynamoDbGateway> _mockGateway;
        private CreateNewApplicationUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IDynamoDbGateway>();
            _classUnderTest = new CreateNewApplicationUseCase(_mockGateway.Object);
        }

        [Test]
        public async Task AddsApplicationToAPI()
        {
            //Arrange
            var pathParameters = _fixture.Create<ApplicationByNameRequest>();
            var bodyParameters = _fixture.Create<CreateApplicationListItem>();
            var api = _fixture.Build<DevelopersHubApi>().With(x => x.Id, pathParameters.Id).Create();
            _mockGateway.Setup(x => x.GetDeveloperHubById(pathParameters.Id)).ReturnsAsync(api);
            //Act
            var result = await _classUnderTest.Execute(pathParameters, bodyParameters).ConfigureAwait(false);
            //Assert
            _mockGateway.Verify(x => x.SaveDeveloperHub(It.IsAny<DevelopersHubApi>()), Times.Once());
            result.Applications.Should().Contain(x => x.Name == bodyParameters.Name);
        }

        [Test]
        public void CreateNewApplicationUseCaseAsyncExceptionIsThrown()
        {
            // Arrange
            var pathParameters = _fixture.Create<ApplicationByNameRequest>();
            var bodyParameters = _fixture.Create<CreateApplicationListItem>();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetDeveloperHubById(pathParameters.Id)).ThrowsAsync(exception);
            // Act
            Func<Task<DevelopersHubApi>> func = async () => await _classUnderTest.Execute(pathParameters, bodyParameters).ConfigureAwait(false);
            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);

        }

        [Test]

        public async Task CreateNewApplicationUseCaseReturnsNullIfAPIDoesNotExist()
        {
            // Arrange
            var pathParameters = _fixture.Create<ApplicationByNameRequest>();
            var bodyParameters = _fixture.Create<CreateApplicationListItem>();
            _mockGateway.Setup(x => x.GetDeveloperHubById(pathParameters.Id)).ReturnsAsync((DevelopersHubApi) null);
            // Act
            var result = await _classUnderTest.Execute(pathParameters, bodyParameters).ConfigureAwait(false);
            // Assert 
            result.Should().BeNull();
        }
    }
}
