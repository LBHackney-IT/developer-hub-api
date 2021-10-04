// using System.Linq;
// using AutoFixture;
// using DeveloperHubAPI.V1.Boundary.Response;
// using DeveloperHubAPI.V1.Domain;
// using DeveloperHubAPI.V1.Factories;
// using DeveloperHubAPI.V1.Gateways;
// using DeveloperHubAPI.V1.UseCase;
// using FluentAssertions;
// using Moq;
// using NUnit.Framework;

// namespace DeveloperHubAPI.Tests.V1.UseCase
// {
//     public class GetAllDeveloperHubUseCaseTests
//     {
//         private Mock<IDeveloperHubGateway> _mockGateway;
//         private GetAllDeveloperHubUseCase _classUnderTest;
//         private Fixture _fixture;

//         [SetUp]
//         public void SetUp()
//         {
//             _mockGateway = new Mock<IExampleGateway>();
//             _classUnderTest = new GetAllUseCase(_mockGateway.Object);
//             _fixture = new Fixture();
//         }

//         [Test]
//         public void GetsAllFromTheGateway()
//         {
//             var stubbedEntities = _fixture.CreateMany<Entity>().ToList();
//             _mockGateway.Setup(x => x.GetAll()).Returns(stubbedEntities);

//             var expectedResponse = new ResponseObjectList { ResponseObjects = stubbedEntities.ToResponse() };

//             _classUnderTest.Execute().Should().BeEquivalentTo(expectedResponse);
//         }

//         //TODO: Add extra tests here for extra functionality added to the use case
//     }
// }
