using AutoFixture;
using DeveloperHubAPI.Tests.V1.Helper;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
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
    public class DeveloperHubAPIControllerTests : LogCallTestContext
    {
        private Mock<IGetDeveloperHubByIdUseCase> _mockGetDeveloperHubByIdUseCase;
        private Mock<IGetApplicationByNameUseCase> _mockGetApplicationByNameUseCase;

        private Mock<IDeleteApplicationByNameUseCase> _mockDeleteApplicationByNameUseCase;
        private Mock<IUpdateApplicationUseCase> _mockUpdateApplicationUseCase;
        private DeveloperHubAPIController _classUnderTest;
        private Fixture _fixture = new Fixture();

        [SetUp]
        public void Init()
        {
            _mockGetDeveloperHubByIdUseCase = new Mock<IGetDeveloperHubByIdUseCase>();
            _mockGetApplicationByNameUseCase = new Mock<IGetApplicationByNameUseCase>();
            _mockDeleteApplicationByNameUseCase = new Mock<IDeleteApplicationByNameUseCase>();
            _mockUpdateApplicationUseCase = new Mock<IUpdateApplicationUseCase>();

            _classUnderTest = new DeveloperHubAPIController(_mockGetDeveloperHubByIdUseCase.Object, _mockGetApplicationByNameUseCase.Object, _mockDeleteApplicationByNameUseCase.Object, _mockUpdateApplicationUseCase.Object);
        }

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = "1" };
        }

        private static DeleteApplicationByNameRequest DeletionQuery()
        {
            return new DeleteApplicationByNameRequest() { Id = "1", ApplicationName = "TestApp" };
        }

        private (ApplicationByNameRequest, UpdateApplicationListItem) ConstructUpdateApplicationQuery()
        {
            var pathParameters = _fixture.Create<ApplicationByNameRequest>();
            var bodyParameters = _fixture.Create<UpdateApplicationListItem>();
            return (pathParameters, bodyParameters);
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

        [Test]
        public async Task DeleteApplicationReturnsNotFound()
        {
            var query = DeletionQuery();
            _mockDeleteApplicationByNameUseCase.Setup(x => x.Execute(query)).ReturnsAsync((ApplicationResponse) null);

            var response = await _classUnderTest.DeleteApplication(query).ConfigureAwait(false);
            response.Should().BeOfType(typeof(NotFoundObjectResult));
        }

        [Test]
        public async Task DeleteApplicationReturnsOkResponse()
        {
            var query = DeletionQuery();
            var applicationResponse = _fixture.Create<ApplicationResponse>();

            _mockDeleteApplicationByNameUseCase.Setup(x => x.Execute(query)).ReturnsAsync(applicationResponse);

            var response = await _classUnderTest.DeleteApplication(query).ConfigureAwait(false);
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().BeEquivalentTo(applicationResponse);
        }

        [Test]
        public void DeleteApplicationThrowsException()
        {
            var query = DeletionQuery();
            var exception = new ApplicationException("Test Exception");
            _mockDeleteApplicationByNameUseCase.Setup(x => x.Execute(query)).ThrowsAsync(exception);

            Func<Task<IActionResult>> func = async () => await _classUnderTest.DeleteApplication(query).ConfigureAwait(false);

            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        public async Task UpdateApplicationAsyncReturnsNoContentResponse()
        {
            // Arrange
            (var pathParameters, var bodyParameters) = ConstructUpdateApplicationQuery();
            var api = _fixture.Create<DevelopersHubApi>();
            _mockUpdateApplicationUseCase.Setup(x => x.Execute(pathParameters, bodyParameters)).ReturnsAsync((DevelopersHubApi) api);

            // Act
            var response = await _classUnderTest.PatchApplication(pathParameters, bodyParameters).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NoContentResult));
        }
        [Test]
        public async Task UpdateApplicationAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            (var pathParameters, var bodyParameters) = ConstructUpdateApplicationQuery();
            _mockUpdateApplicationUseCase.Setup(x => x.Execute(pathParameters, bodyParameters)).ReturnsAsync((DevelopersHubApi) null);

            // Act
            var response = await _classUnderTest.PatchApplication(pathParameters, bodyParameters).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(pathParameters.Id);
        }
    }
}
