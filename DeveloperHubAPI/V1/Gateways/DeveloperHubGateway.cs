using System.Collections.Generic;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;

namespace DeveloperHubAPI.V1.Gateways
{
    public class DeveloperHubGateway : IDeveloperHubGateway
    {
        private readonly DatabaseContext _databaseContext;

        public DeveloperHubGateway(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public DeveloperHub GetDeveloperHubById(DeveloperHubQuery query)
        {
            var result = _databaseContext.DatabaseEntities.Find(query);

            return result?.ToDomain();
        }

        // public List<DeveloperHub> GetAll()
        // {
        //     return new List<DeveloperHub>();
        // }
    }
}
