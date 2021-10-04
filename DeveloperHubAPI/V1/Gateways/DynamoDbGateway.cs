using Amazon.DynamoDBv2.DataModel;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.Gateways
{
    public class DynamoDbGateway : IDeveloperHubGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<DeveloperHub> GetDeveloperHubById(DeveloperHubQuery query)
        {
            var result = await _dynamoDbContext.LoadAsync<DeveloperHub>(query.Id).ConfigureAwait(false); 
            return result;
        }

        // public List<DeveloperHub> GetAll()
        // {
        //     return new List<DeveloperHub>();
        // }
    }
}
