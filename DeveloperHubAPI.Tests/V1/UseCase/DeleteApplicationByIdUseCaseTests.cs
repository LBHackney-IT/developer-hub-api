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
using DeveloperHubAPI.Tests.V1.Helper;

namespace DeveloperHubAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class DeleteApplicationByIdUseCaseTests : LogCallTestContext
    {
        private Mock<IDynamoDbGateway> _mockGateway;
        private DeleteApplicationByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IDynamoDbGateway>();
            _classUnderTest = new DeleteApplicationByIdUseCase(_mockGateway.Object);
        }

        [Test]
        public async Task DeleteApplicationByIdUseCaseReturnsNullWhenApiIsNotFound()
        {

            var application = _fixture.Create<Application>();
            var query = _fixture.Build<ApplicationByIdRequest>().With(x => x.ApplicationId, application.Id).Create();

            var api = _fixture.Create<DevelopersHubApi>();
            api.Applications.Add(application);

            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ReturnsAsync((DevelopersHubApi) null);

            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);
            response.Should().BeNull();

        }

        [Test]
        public async Task DeleteApplicationByIdUseCaseReturnsOkResponse()
        {
            var application = _fixture.Create<Application>();
            var query = _fixture.Build<ApplicationByIdRequest>().With(x => x.ApplicationId, application.Id).Create();
            var api = _fixture.Create<DevelopersHubApi>();

            api.Applications.Add(application);

            _mockGateway.Setup(x => x.GetDeveloperHubById(query.Id)).ReturnsAsync(api);

            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);
            response.Should().BeEquivalentTo(application);
        }

    }
}
