using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using Hackney.Core.Logging;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.UseCase
{
    public class GetDeveloperHubByIdUseCase : IGetDeveloperHubByIdUseCase
    {
        private IDynamoDbGateway _gateway;

        public GetDeveloperHubByIdUseCase(IDynamoDbGateway gateway)
        {
            _gateway = gateway;
        }

        [LogCall]
        public async Task<DevelopersHubApi> Execute(DeveloperHubQuery query)
        {
            return await _gateway.GetDeveloperHubById(query.Id).ConfigureAwait(false);
        }
    }
}
