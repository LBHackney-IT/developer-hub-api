using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using Hackney.Core.Logging;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.UseCase
{
    public class CreateNewApplicationUseCase : ICreateNewApplicationUseCase
    {
        private IDynamoDbGateway _gateway;

        public CreateNewApplicationUseCase(IDynamoDbGateway gateway)
        {
            _gateway = gateway;
        }

        [LogCall]
        public async Task<DevelopersHubApi> Execute(ApplicationByNameRequest pathParameters, CreateApplicationListItem bodyParameters)
        {
            var api = await _gateway.GetDeveloperHubById(pathParameters.Id).ConfigureAwait(false);
            if (api == null)
                return null;
            var applicationData = new Application()
            {
                Name = bodyParameters.Name,
                Link = bodyParameters.Link
            };
            api.Applications.Add(applicationData);
            await _gateway.SaveDeveloperHub(api).ConfigureAwait(false);
            return api;
        }
    }
}
