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
using Microsoft.Extensions.Logging;
using Hackney.Core.Logging;
using Hackney.Core.Testing.Shared;

namespace DeveloperHubAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class GetDeveloperHubByIdUseCaseTests
    {
        private Mock<IDynamoDbGateway> _mockGateway;
        private GetDeveloperHubByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();
        public Mock<ILogger<LogCallAspect>> MockLogger { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            MockLogger = LogCallAspectFixture.SetupLogCallAspect();
        }


        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IDynamoDbGateway>();
            _classUnderTest = new GetDeveloperHubByIdUseCase(_mockGateway.Object);
        }

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = "1" };
        }

        [Test]
        public async Task GetDeveloperHubByIdUseCaseGatewayReturnsNullReturnsNull()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ReturnsAsync((DevelopersHubApi) null);

            // Act
            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
        }

        [Test]
        public async Task GetDeveloperHubByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var query = ConstructQuery();
            var developerHubApi = _fixture.Create<DevelopersHubApi>();
            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ReturnsAsync(developerHubApi);

            // Act
            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(developerHubApi);
        }

        [Test]
        public void GetDeveloperHubByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var query = ConstructQuery();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ThrowsAsync(exception);

            // Act
            Func<Task<DevelopersHubApi>> func = async () => await _classUnderTest.Execute(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
