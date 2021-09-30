using System.Collections.Generic;
using DeveloperHubAPI.V1.Domain;

namespace DeveloperHubAPI.V1.Gateways
{
    public interface IDeveloperHubGateway
    {
        DeveloperHub GetDeveloperHubById(DeveloperHubQuery query);

        // public List<DeveloperHub> GetAll()
    }
}
