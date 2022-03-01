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
<<<<<<< HEAD

        private Mock<IDeleteApplicationByNameUseCase> _mockDeleteApplicationByNameUseCase;
=======
        private Mock<ICreateNewApplicationUseCase> _mockCreateNewApplicationUseCase;
>>>>>>> origin/adding-new-application-endpoint
        private DeveloperHubAPIController _classUnderTest;
        private Fixture _fixture = new Fixture();

        [SetUp]
        public void Init()
        {
            _mockGetDeveloperHubByIdUseCase = new Mock<IGetDeveloperHubByIdUseCase>();
            _mockGetApplicationByNameUseCase = new Mock<IGetApplicationByNameUseCase>();
<<<<<<< HEAD
            _mockDeleteApplicationByNameUseCase = new Mock<IDeleteApplicationByNameUseCase>();

            _classUnderTest = new DeveloperHubAPIController(_mockGetDeveloperHubByIdUseCase.Object, _mockGetApplicationByNameUseCase.Object, _mockDeleteApplicationByNameUseCase.Object);
=======
            _mockCreateNewApplicationUseCase = new Mock<ICreateNewApplicationUseCase>();

            _classUnderTest = new DeveloperHubAPIController(_mockGetDeveloperHubByIdUseCase.Object, _mockGetApplicationByNameUseCase.Object, _mockCreateNewApplicationUseCase.Object);
>>>>>>> origin/adding-new-application-endpoint
        }

        private static DeveloperHubQuery ConstructQuery()
        {
            return new DeveloperHubQuery() { Id = "1" };
        }

<<<<<<< HEAD
        private static DeleteApplicationByNameRequest DeletionQuery()
        {
            return new DeleteApplicationByNameRequest() { Id = "1", ApplicationName = "TestApp" };
=======
        private (ApplicationByNameRequest, CreateApplicationListItem) ConstructCreateApplicationQuery()
        {
            var pathParameters = _fixture.Create<ApplicationByNameRequest>();
            var bodyParameters = _fixture.Create<CreateApplicationListItem>();
            return (pathParameters, bodyParameters);
>>>>>>> origin/adding-new-application-endpoint
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
            (response as NotFoundObjectResult).Value.Should().BeEquivalentTo(query.ApplicationName);
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

        public async Task CreateNewApplicationAsyncReturnsCreatedResponse()
        {
            // Arrange
            (var pathParameters, var bodyParameters) = ConstructCreateApplicationQuery();
            var api = _fixture.Create<DevelopersHubApi>();
            _mockCreateNewApplicationUseCase.Setup(x => x.Execute(pathParameters, bodyParameters)).ReturnsAsync((DevelopersHubApi) api);

            // Act
            var response = await _classUnderTest.PostApplication(pathParameters, bodyParameters).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(CreatedResult));
            (response as CreatedResult).Value.Should().Be(api);
        }
        [Test]
        public async Task CreateNewApplicationAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            (var pathParameters, var bodyParameters) = ConstructCreateApplicationQuery();
            _mockCreateNewApplicationUseCase.Setup(x => x.Execute(pathParameters, bodyParameters)).ReturnsAsync((DevelopersHubApi) null);

            // Act
            var response = await _classUnderTest.PostApplication(pathParameters, bodyParameters).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(pathParameters.Id);
        }
    }
}
