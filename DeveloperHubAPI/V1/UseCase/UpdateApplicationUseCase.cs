using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Gateways;
using DeveloperHubAPI.V1.UseCase.Interfaces;
using Hackney.Core.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.UseCase
{
    public class UpdateApplicationUseCase : IUpdateApplicationUseCase
    {
        private IDynamoDbGateway _gateway;

        public UpdateApplicationUseCase(IDynamoDbGateway gateway)
        {
            _gateway = gateway;
        }

        [LogCall]
        public async Task<DevelopersHubApi> Execute(ApplicationByNameRequest pathParameters, UpdateApplicationListItem bodyParameters)
        {
            var api = await _gateway.GetDeveloperHubById(pathParameters.Id).ConfigureAwait(false);
            if (api == null)
                return null;
            var doesApplicationExist = api.Applications.Find(x => x.Name == pathParameters.ApplicationName);
            if (doesApplicationExist != null)
            {
                var applicationData = new Application()
                {
                    Name = bodyParameters.Name ?? doesApplicationExist.Name,
                    Link = bodyParameters.Link ?? doesApplicationExist.Link
                };
                api.Applications.Add(applicationData);
            }
            else
            {

                var applicationData = new Application()
                {
                    Name = bodyParameters.Name,
                    Link = bodyParameters.Link
                };
                api.Applications.Add(applicationData);
            }
            await _gateway.SaveDeveloperHub(api).ConfigureAwait(false);
            return api;
        }
    }
}
