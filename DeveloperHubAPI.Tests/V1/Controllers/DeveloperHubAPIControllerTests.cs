using AutoFixture;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Controllers;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DynamodbTraining.Tests.V1.Controllers
{
    public class DynamodbTrainingControllerTests
    {
        private readonly Mock<IGetDeveloperHubByIdUseCase> _mockGetDeveloperHubByIdUseCase;
        private readonly DeveloperHubAPIController _classUnderTest;
        private readonly Fixture _fixture = new Fixture();


        public DynamodbTrainingControllerTests()
        {
            _mockGetDeveloperHubByIdUseCase = new Mock<IGetDeveloperHubByIdUseCase>();

            _classUnderTest = new DeveloperHubAPIController(_mockGetDeveloperHubByIdUseCase.Object);
        }

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = Guid.NewGuid().ToString() };
        }

        [Fact]
        public void GetDeveloperHubByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGetDeveloperHubByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync((DeveloperHub) null);

            // Act
            var response = _classUnderTest.ViewDeveloperHub(query);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(query.Id);
        }

        [Fact]
        public void GetDeveloperHubByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var query = ConstructQuery();
            var developerHubResponse = _fixture.Create<DeveloperHub>();
            _mockGetDeveloperHubByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync(developerHubResponse);

            // Act
            var response = _classUnderTest.ViewDeveloperHub(query);

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
