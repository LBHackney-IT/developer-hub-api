using FluentValidation.TestHelper;
using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Request.Validation;
using NUnit.Framework;

namespace DeveloperHubAPI.Tests.V1.Boundary.Request.Validation
{
    [TestFixture]
    public class GetDeveloperHubByIdValidatorTests
    {
        private readonly GetDeveloperHubByIdValidator _sut;

        public GetDeveloperHubByIdValidatorTests()
        {
            _sut = new GetDeveloperHubByIdValidator();
        }

        [Test]
        public void QueryShouldErrorWithEmptyId()
        {
            var query = new DeveloperHubQuery() { Id = string.Empty };
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void QueryShouldErrorWithNullId()
        {
            var query = new DeveloperHubQuery();
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}
