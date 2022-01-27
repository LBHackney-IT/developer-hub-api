using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
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

        public async Task<ApplicationResponse> Execute(ApplicationByNameRequest query)
        {
            var api = await _gateway.GetDeveloperHubById(query.Id).ConfigureAwait(false);
            var application = api.Applications.Find(x => x.Name == query.ApplicationName);

            return application?.ToResponse();
        }
    }
}
