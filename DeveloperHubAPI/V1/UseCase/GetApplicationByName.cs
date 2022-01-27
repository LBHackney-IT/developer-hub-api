using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.UseCase
{
    public class GetApplicationByNameUseCase : IGetApplicationByNameUseCase
    {
        private IDynamoDbGateway _gateway;

        public GetApplicationByNameUseCase(IDynamoDbGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Application> Execute(ApplicationByNameRequest query)
        {
           var api = await _gateway.GetDeveloperHubById(query.Id).ConfigureAwait(false);
           return api.Applications.Find( x => x.Name == query.ApplicationName);
        }
    }
}
