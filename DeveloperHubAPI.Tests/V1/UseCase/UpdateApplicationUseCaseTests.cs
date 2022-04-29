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
    public class UpdateApplicationUseCaseTests : LogCallTestContext
    {
        private Mock<IDynamoDbGateway> _mockGateway;
        private UpdateApplicationByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IDynamoDbGateway>();
            _classUnderTest = new UpdateApplicationByIdUseCase(_mockGateway.Object);
        }

        [Test]
        public async Task AddsApplicationToAPISuccessfully()
        {
            //Arrange
            var pathParameters = _fixture.Create<ApplicationByIdRequest>();
            var bodyParameters = _fixture.Create<UpdateApplicationListItem>();
            var api = _fixture.Build<DevelopersHubApi>().With(x => x.Id, pathParameters.Id).Create();
            _mockGateway.Setup(x => x.GetDeveloperHubById(pathParameters.Id)).ReturnsAsync(api);
            //Act
            var result = await _classUnderTest.Execute(pathParameters, bodyParameters).ConfigureAwait(false);
            //Assert
            _mockGateway.Verify(x => x.SaveDeveloperHub(It.IsAny<DevelopersHubApi>()), Times.Once());
            result.Applications.Should().Contain(x => x.Name == bodyParameters.Name);
        }

        [Test]
        public async Task SuccessfullyUpdatesAnExistingApplication()
        {
            var pathParameters = _fixture.Create<ApplicationByIdRequest>();
            var bodyParameters = _fixture.Create<UpdateApplicationListItem>();
            var api = _fixture.Build<DevelopersHubApi>()
                              .With(x => x.Id, pathParameters.Id)
                              .Create();
            var application = new Application()
            {
                Id = pathParameters.ApplicationId
            };
            api.Applications.Add(application);
            _mockGateway.Setup(x => x.GetDeveloperHubById(pathParameters.Id)).ReturnsAsync(api);
            //Act
            var result = await _classUnderTest.Execute(pathParameters, bodyParameters).ConfigureAwait(false);
            //Assert
            _mockGateway.Verify(x => x.SaveDeveloperHub(It.IsAny<DevelopersHubApi>()), Times.Once());

            result.Applications.Should().Contain(x => x.Id == pathParameters.ApplicationId);
            result.Applications.Should().Contain(x => x.Name == bodyParameters.Name);
            result.Applications.Should().Contain(x => x.Link == bodyParameters.Link);

        }

        [Test]
        public void UpdateApplicationUseCaseAsyncExceptionIsThrown()
        {
            // Arrange
            var pathParameters = _fixture.Create<ApplicationByIdRequest>();
            var bodyParameters = _fixture.Create<UpdateApplicationListItem>();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetDeveloperHubById(pathParameters.Id)).ThrowsAsync(exception);
            // Act
            Func<Task<DevelopersHubApi>> func = async () => await _classUnderTest.Execute(pathParameters, bodyParameters).ConfigureAwait(false);
            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);

        }

        [Test]

        public async Task UpdateApplicationUseCaseReturnsNullIfAPIDoesNotExist()
        {
            // Arrange
            var pathParameters = _fixture.Create<ApplicationByIdRequest>();
            var bodyParameters = _fixture.Create<UpdateApplicationListItem>();
            _mockGateway.Setup(x => x.GetDeveloperHubById(pathParameters.Id)).ReturnsAsync((DevelopersHubApi) null);
            // Act
            var result = await _classUnderTest.Execute(pathParameters, bodyParameters).ConfigureAwait(false);
            // Assert 
            result.Should().BeNull();
        }
    }
}
