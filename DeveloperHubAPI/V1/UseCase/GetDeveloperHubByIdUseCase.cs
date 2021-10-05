using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.UseCase.Interfaces;
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

        public async Task<DeveloperHub> Execute(DeveloperHubQuery query)
        {
            return await _gateway.GetDeveloperHubById(query).ConfigureAwait(false);
        }
    }
}
