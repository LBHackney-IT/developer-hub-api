using Hackney.Core.Logging;
using Hackney.Core.Testing.Shared;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DeveloperHubAPI.Tests.V1.Helper
{
    public class LogCallTestContext
    {
        public Mock<ILogger<LogCallAspect>> MockLogger { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            MockLogger = LogCallAspectFixture.SetupLogCallAspect();
        }
    }
}
