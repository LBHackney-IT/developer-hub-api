using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace DeveloperHubAPI.Tests
{
    public class DynamoDbIntegrationTests<TStartup> where TStartup : class
    {
        public HttpClient Client { get; private set; }
        public IDynamoDBContext DynamoDbContext => _factory?.DynamoDbContext;
        private DynamoDbMockWebApplicationFactory<TStartup> _factory;
        protected List<Action> CleanupActions { get; set; }

        private readonly List<TableDef> _tables = new List<TableDef>
        {

            new TableDef { Name = "DevelopersHubApi", KeyName = "id", KeyType = ScalarAttributeType.S }
        };

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                Environment.SetEnvironmentVariable(name, defaultValue);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            _factory = new DynamoDbMockWebApplicationFactory<TStartup>(_tables);
            Client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory.Dispose();
        }

        [SetUp]
        public void BaseSetup()
        {
            Client = _factory.CreateClient();
            CleanupActions = new List<Action>();
        }

        [TearDown]
        public void BaseTearDown()
        {
            foreach (var act in CleanupActions)
                act();
            Client.Dispose();
        }
    }

    public class TableDef
    {
        public string Name { get; set; }
        public string KeyName { get; set; }
        public ScalarAttributeType KeyType { get; set; }
    }
}
