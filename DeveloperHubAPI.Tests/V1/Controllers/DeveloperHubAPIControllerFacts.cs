using AutoFixture;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Controllers;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeveloperHubAPI.Tests.V1.Controllers
{
    public class DeveloperHubAPIControllerFacts
    {
        private readonly Mock<IGetDeveloperHubByIdUseCase> _mockGetDeveloperHubByIdUseCase;
        private readonly DeveloperHubAPIController _classUnderTest;
        private readonly Fixture _fixture = new Fixture();


        public DeveloperHubAPIControllerFacts()
        {
            _mockGetDeveloperHubByIdUseCase = new Mock<IGetDeveloperHubByIdUseCase>();

            _classUnderTest = new DeveloperHubAPIController(_mockGetDeveloperHubByIdUseCase.Object);
        }

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = "1" }; 
        }

        [Fact]
        public async Task GetDeveloperHubByIdReturnsOkResponse() 
        {
            // Arrange
            var expectedResponse = _fixture.Create<DeveloperHub>();
            var query = ConstructQuery();
            _mockGetDeveloperHubByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync(expectedResponse);
            

            // Act
            var actualResponse = await _classUnderTest.ViewDeveloperHub(query).ConfigureAwait(false) as OkObjectResult;

            // Assert
            actualResponse.Should().NotBeNull();
            actualResponse.StatusCode.Should().Be(200);
            actualResponse.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetDeveloperHubByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGetDeveloperHubByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync((DeveloperHub) null);

            // Act
            var response = await _classUnderTest.ViewDeveloperHub(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(query.Id);
        }

        [Fact]
        public async Task GetDeveloperHubByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var query = ConstructQuery();
            var developerHubResponse = _fixture.Create<DeveloperHub>();
            _mockGetDeveloperHubByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync(developerHubResponse);

            // Act
            var response = await _classUnderTest.ViewDeveloperHub(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().BeEquivalentTo(developerHubResponse);
        }

        [Fact]
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
