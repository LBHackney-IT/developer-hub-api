using Amazon.DynamoDBv2.DataModel;
using DeveloperHubAPI.V1.Domain;
using DeveloperHubAPI.V1.Factories;
using DeveloperHubAPI.V1.Infrastructure;
using Hackney.Core.Logging;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.Gateways
{
    public class DynamoDbGateway : IDynamoDbGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<DynamoDbGateway> _logger;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, ILogger<DynamoDbGateway> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        [LogCall]
        public async Task<DevelopersHubApi> GetDeveloperHubById(string id)
        {
            _logger.LogDebug($"Calling IDynamoDBContext.LoadAsync for Developer Hub API ID: {id}");

            var result = await _dynamoDbContext.LoadAsync<DeveloperHubDb>(id).ConfigureAwait(false);
            return result?.ToDomain();
        }

        [LogCall]

        public async Task SaveDeveloperHub(DevelopersHubApi api)
        {
            _logger.LogDebug($"Calling IDynamoDBContext.SaveAsync for Developer Hub API: {api.Id}");
            var databaseAPi = api.ToDatabase();
            await _dynamoDbContext.SaveAsync<DeveloperHubDb>(databaseAPi).ConfigureAwait(false);
        }
    }
}
