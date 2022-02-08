using AutoFixture;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Controllers;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using FluentAssertions;
using Hackney.Core.Logging;
using Hackney.Core.Testing.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private Mock<IGetApplicationByNameUseCase> _mockGetApplicationByNameUseCase;
        private DeveloperHubAPIController _classUnderTest;
        private Fixture _fixture = new Fixture();
        public Mock<ILogger<LogCallAspect>> MockLogger { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            MockLogger = LogCallAspectFixture.SetupLogCallAspect();
        }


        [SetUp]
        public void Init()
        {
            _mockGetDeveloperHubByIdUseCase = new Mock<IGetDeveloperHubByIdUseCase>();
            _mockGetApplicationByNameUseCase = new Mock<IGetApplicationByNameUseCase>();

            _classUnderTest = new DeveloperHubAPIController(_mockGetDeveloperHubByIdUseCase.Object, _mockGetApplicationByNameUseCase.Object);
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

        [Test]

        public async Task GetApplicationByIdReturnsTheApplication()
        {
            //Arrange

            var application = _fixture.Create<ApplicationResponse>();
            var query = _fixture.Build<ApplicationByNameRequest>().With(x => x.ApplicationName, application.Name).Create();
            _mockGetApplicationByNameUseCase.Setup(x => x.Execute(query)).ReturnsAsync(application);

            //Act

            var result = await _classUnderTest.GetApplication(query).ConfigureAwait(false) as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(application);
        }
        [Test]
        public async Task GetApplicationByNameReturnsNotFoundAsync()
        {
            // Arrange             
            var application = _fixture.Create<ApplicationResponse>();
            var query = _fixture.Create<ApplicationByNameRequest>();
            // Act
            var result = await _classUnderTest.GetApplication(query).ConfigureAwait(false) as NotFoundObjectResult;
            // Assert
            result.Value.Should().Be(query.ApplicationName);
            result.StatusCode.Should().Be(404);
        }
        [Test]
        public void GetApplicationByNameExceptionIsThrown()
        {
            // Arrange

            var application = _fixture.Create<ApplicationResponse>();
            var query = _fixture.Create<ApplicationByNameRequest>();
            var exception = _fixture.Create<ApplicationException>();
            _mockGetApplicationByNameUseCase.Setup(x => x.Execute(query)).ThrowsAsync(exception);

            // Act 

            Func<Task<IActionResult>> func = () => (Task<IActionResult>) _classUnderTest.GetApplication(query);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

    }
}
