using System;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.UseCase;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Boundary.Request;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using AutoFixture;

namespace DeveloperHubAPI.Tests.V1.UseCase
{
    public class GetDeveloperHubByIdUseCaseTests
    {
        private Mock<IDynamoDbGateway> _mockGateway;
        private GetDeveloperHubByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        
        public GetDeveloperHubByIdUseCaseTests()
        {
            _mockGateway = new Mock<IDynamoDbGateway>();
            _classUnderTest = new GetDeveloperHubByIdUseCase(_mockGateway.Object);
        }

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = "1"};
        }

        [Fact]
        public async Task GetDeveloperHubByIdUseCaseGatewayReturnsNullReturnsNull()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGateway.Setup(x => x.GetDeveloperHubById(query)).ReturnsAsync((DeveloperHub) null);

            // Act
            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetDeveloperHubByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var query = ConstructQuery();
            var developerHub = _fixture.Create<DeveloperHub>();
            _mockGateway.Setup(x => x.GetDeveloperHubById(query)).ReturnsAsync(developerHub);

            // Act
            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(developerHub);
        }

        [Fact]
        public void GetDeveloperHubByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var query = ConstructQuery();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetDeveloperHubById(query)).ThrowsAsync(exception);

            // Act
            Func<Task<DeveloperHub>> func = async () => await _classUnderTest.Execute(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
