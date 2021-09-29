using Amazon.DynamoDBv2.DataModel;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;
using System.Collections.Generic;

namespace DeveloperHubAPI.V1.Gateways
{
    public class DynamoDbGateway : IExampleGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        // public List<DeveloperHub> GetAll()
        // {
        //     return new List<DeveloperHub>();
        // }

        public async Task<DeveloperHub> GetEntityById(DeveloperHubQuery query)
        {
            var result = await _dynamoDbContext.LoadAsync<DeveloperHub>(query.Id).ConfigureAwait(false); 
            return result?.ToDomain();
        }
    }
}
