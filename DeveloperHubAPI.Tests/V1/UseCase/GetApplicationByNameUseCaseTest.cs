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

namespace DeveloperHubAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class GetApplicationByNameUseCaseTests
    {
        private Mock<IDynamoDbGateway> _mockGateway;
        private GetApplicationByNameUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IDynamoDbGateway>();
            _classUnderTest = new GetApplicationByNameUseCase(_mockGateway.Object);
        }

        [Test]
        public async Task GetApplicationByNameUseCaseReturnsApplicationFromTheQuery()
        {
            //Arrange
            var application = _fixture.Create<Application>();
            var query = _fixture.Build<ApplicationByNameRequest>().With(x => x.ApplicationName, application.Name).Create();

            var api = _fixture.Create<DevelopersHubApi>();
            api.Applications.Add(application);

            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ReturnsAsync(api);
            //Act
            var result = await _classUnderTest.Execute(query).ConfigureAwait(false);
            //Assert
            result.Should().BeEquivalentTo(application.ToResponse());
        }

        [Test]
        public void GetApplicationByNameUseCaseAsyncExceptionIsThrown()
        {
            // Arrange
            var application = _fixture.Create<DevelopersHubApi>();
            var query = _fixture.Create<ApplicationByNameRequest>();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ThrowsAsync(exception);
            // Act
            Func<Task<ApplicationResponse>> func = async () => await _classUnderTest.Execute(query).ConfigureAwait(false);
            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);

        }

        [Test]

        public async Task GetApplicationByNameUseCaseReturnsNullIfApplicationDoesNotExist()
        {
            // Arrange
            var application = _fixture.Create<Application>();
            var query = _fixture.Create<ApplicationByNameRequest>();
            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ReturnsAsync((DevelopersHubApi) null);
            // Act
            var result = await _classUnderTest.Execute(query).ConfigureAwait(false);
            // Assert 
            result.Should().BeNull();
        }
    }
}
