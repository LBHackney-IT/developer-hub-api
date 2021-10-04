using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Domain;

namespace DeveloperHubAPI.V1.Gateways
{
    public interface IDeveloperHubGateway
    {
        Task<DeveloperHub> GetDeveloperHubById(DeveloperHubQuery query);

        // public List<DeveloperHub> GetAll()
    }
}
