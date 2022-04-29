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
    public class GetApplicationByNameUseCaseTests : LogCallTestContext
    {
        private Mock<IDynamoDbGateway> _mockGateway;
        private GetApplicationByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IDynamoDbGateway>();
            _classUnderTest = new GetApplicationByIdUseCase(_mockGateway.Object);
        }

        [Test]
        public async Task GetApplicationByIdUseCaseReturnsApplicationFromTheQuery()
        {
            //Arrange
            var application = _fixture.Create<Application>();
            var query = _fixture.Build<ApplicationByIdRequest>().With(x => x.ApplicationId, application.Id).Create();

            var api = _fixture.Create<DevelopersHubApi>();
            api.Applications.Add(application);

            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ReturnsAsync(api);
            //Act
            var result = await _classUnderTest.Execute(query).ConfigureAwait(false);
            //Assert
            result.Should().BeEquivalentTo(application.ToResponse());
        }

        [Test]
        public void GetApplicationByIdUseCaseAsyncExceptionIsThrown()
        {
            // Arrange
            var application = _fixture.Create<DevelopersHubApi>();
            var query = _fixture.Create<ApplicationByIdRequest>();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ThrowsAsync(exception);
            // Act
            Func<Task<ApplicationResponse>> func = async () => await _classUnderTest.Execute(query).ConfigureAwait(false);
            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);

        }

        [Test]

        public async Task GetApplicationByIdUseCaseReturnsNullIfApplicationDoesNotExist()
        {
            // Arrange
            var application = _fixture.Create<Application>();
            var query = _fixture.Create<ApplicationByIdRequest>();
            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ReturnsAsync((DevelopersHubApi) null);
            // Act
            var result = await _classUnderTest.Execute(query).ConfigureAwait(false);
            // Assert 
            result.Should().BeNull();
        }
    }
}
