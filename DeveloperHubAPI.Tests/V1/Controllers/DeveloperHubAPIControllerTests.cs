using AutoFixture;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Controllers;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DeveloperHubAPI.Tests.V1.Controllers
{
    [TestFixture]
    public class DeveloperHubAPIControllerTests
    {
        private Mock<IGetDeveloperHubByIdUseCase> _mockGetDeveloperHubByIdUseCase;
        private DeveloperHubAPIController _classUnderTest;
        private Fixture _fixture = new Fixture();

        [SetUp]
        public void Init()
        {
            _mockGetDeveloperHubByIdUseCase = new Mock<IGetDeveloperHubByIdUseCase>();

            _classUnderTest = new DeveloperHubAPIController(_mockGetDeveloperHubByIdUseCase.Object);
        }

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = "1" };
        }

        [Test]
        public async Task GetDeveloperHubByIdReturnsOkResponse()
        {
            // Arrange
            var expectedResponse = _fixture.Create<DevelopersHubApi>();
            var query = ConstructQuery();
            _mockGetDeveloperHubByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync(expectedResponse);


            // Act
            var actualResponse = await _classUnderTest.ViewDeveloperHub(query).ConfigureAwait(false) as OkObjectResult;

            // Assert
            actualResponse.Should().NotBeNull();
            actualResponse.StatusCode.Should().Be(200);
            actualResponse.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public async Task GetDeveloperHubByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGetDeveloperHubByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync((DevelopersHubApi) null);

            // Act
            var response = await _classUnderTest.ViewDeveloperHub(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(query.Id);
        }

        [Test]
        public void GetDeveloperHubByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var query = ConstructQuery();
            var exception = new ApplicationException("Test exception");
            _mockGetDeveloperHubByIdUseCase.Setup(x => x.Execute(query)).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = () => (Task<IActionResult>) _classUnderTest.ViewDeveloperHub(query);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

    }
}
