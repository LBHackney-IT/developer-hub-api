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
        public void QueryShouldErrorWithEmptyTargetId()
        {
            var query = new DeveloperHubQuery() { Id = string.Empty };
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void QueryShouldErrorWithNullTargetId()
        {
            var query = new DeveloperHubQuery();
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}
