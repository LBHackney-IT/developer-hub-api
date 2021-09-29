using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.UseCase.Interfaces;

namespace DeveloperHubAPI.V1.UseCase
{
    public class GetDeveloperHubByIdUseCase : IGetByIdUseCase
    {
        private IExampleGateway _gateway;
        public GetByIdUseCase(IExampleGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<DeveloperHub> Execute(DeveloperHubQuery query)
        {
            return await _gateway.GetEntityById(query).ConfigureAwait(false);
        }
    }
}
