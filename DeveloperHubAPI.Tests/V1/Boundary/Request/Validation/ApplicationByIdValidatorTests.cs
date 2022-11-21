using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Request.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DeveloperHubAPI.Tests.V1.Boundary.Request.Validation
{
    [TestFixture]
    public class ApplicationByIdValidatorTests
    {
        private readonly ApplicationByIdValidation _sut;

        public ApplicationByIdValidatorTests()
        {
            _sut = new ApplicationByIdValidation();
        }

        [Test]
        public void QueryShouldErrorWithEmptyId()
        {
            var query = new ApplicationByIdRequest() { Id = string.Empty };
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void QueryShouldErrorWithNullId()
        {
            var query = new ApplicationByIdRequest();
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}
