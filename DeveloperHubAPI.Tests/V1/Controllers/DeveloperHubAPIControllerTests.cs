using AutoFixture;
using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Controllers;
using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.UseCase.Interfaces;
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
        private readonly Mock<IDeveloperHubByIdCase> _mockGetDeveloperHubByIdUseCase;
        private readonly DeveloperHubAPIController _classUnderTest;
        private readonly ResponseFactory _responseFactory;
        private readonly Fixture _fixture = new Fixture();


        public DeveloperHubAPIControllerTests()
        {
            _mockGetDeveloperHubByIdUseCase = new Mock<IGetDeveloperHubByIdUseCase>();
            _responseFactory = new ResponseFactory();

            _classUnderTest = new DeveloperHubAPIController(_mockGetDeveloperHubByIdUseCase.Object);
        }

        private DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = Guid.NewGuid() };
        }

        [Fact]
        public async Task GetDeveloperHubByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGetDeveloperHubByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync((DeveloperHub) null);

            // Act
            var response = await _classUnderTest.ViewRecord(query).ConfigureAwait(false);

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
            _mockGetByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync(developerHubResponse);

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
            Func<Task<IActionResult>> func = async () => await _classUnderTest.ViewDeveloperHub(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

    }
}