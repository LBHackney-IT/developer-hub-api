using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.UseCase.Interfaces;

namespace DeveloperHubAPI.V1.UseCase
{
    public class GetDeveloperHubByIdUseCase : IGetDeveloperHubByIdUseCase
    {
        private IDeveloperHubGateway _gateway;
        
        public GetDeveloperHubByIdUseCase(IDeveloperHubGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<DeveloperHub> Execute(DeveloperHubQuery query)
        {
            return await _gateway.GetDeveloperHubById(query).ConfigureAwait(false);
        }
    }
}
