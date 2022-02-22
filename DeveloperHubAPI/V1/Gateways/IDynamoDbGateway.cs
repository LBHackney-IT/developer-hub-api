using System.Threading.Tasks;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Domain;

namespace DeveloperHubAPI.V1.Gateways
{
    public interface IDynamoDbGateway
    {
        Task<DevelopersHubApi> GetDeveloperHubById(string id);

        Task<DevelopersHubApi> DeleteApplication(DeleteApplicationByNameRequest query);
    }
}
