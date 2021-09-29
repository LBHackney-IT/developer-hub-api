using System.Collections.Generic;
using DeveloperHubAPI.V1.Domain;

namespace DeveloperHubAPI.V1.Gateways
{
    public interface IExampleGateway
    {
        Task<DeveloperHub> GetEntityById(DeveloperHubQuery query);

        // List<Entity> GetAll();
    }
}
