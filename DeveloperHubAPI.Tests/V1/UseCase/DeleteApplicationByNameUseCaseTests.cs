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
    public class DeleteApplicationByNameUseCaseTests
    {
        private Mock<IDynamoDbGateway> _mockGateway;
        private DeleteApplicationByNameUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IDynamoDbGateway>();
            _classUnderTest = new DeleteApplicationByNameUseCase(_mockGateway.Object);
        }

        [Test]
        public async Task DeleteApplicationByNameUseCaseIsNull()
        {
            
            var application = _fixture.Create<Application>();
            var query = _fixture.Build<DeleteApplicationByNameRequest>().With(x => x.ApplicationName, application.Name).Create();

            var api = _fixture.Create<DevelopersHubApi>();
            api.Applications.Add(application);

            _mockGateway.Setup(x => x.DeleteApplication(query)).ReturnsAsync((DevelopersHubApi) null);
            
            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);
            response.Should().BeNull();
        
        }

        [Test]
        public async Task DeleteApplicationByNameUseCaseReturnsOkResponse()
        {   
            var application = _fixture.Create<Application>();
            var query = _fixture.Build<DeleteApplicationByNameRequest>().With(x => x.ApplicationName, application.Name).Create();

            var api = _fixture.Create<DevelopersHubApi>();
            api.Applications.Add(application);
           
            _mockGateway.Setup(x => x.DeleteApplication(query)).ReturnsAsync(api);
            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);
            
            response.Should().BeEquivalentTo(api.ToResponse());
        }

        [Test]
        public void DeleteApplicationByNameUseThrowsException()
        {   
            var application = _fixture.Create<Application>();
            var query = _fixture.Build<DeleteApplicationByNameRequest>().With(x => x.ApplicationName, application.Name).Create();

            var api = _fixture.Create<DevelopersHubApi>();
            api.Applications.Add(application);

            var exception = new ApplicationException("Test Exception");
            _mockGateway.Setup(x => x.DeleteApplication(query)).ThrowsAsync(exception);

            Func<Task<ApplicationResponse>> func = async () => await _classUnderTest.Execute(query).ConfigureAwait(false);

            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}