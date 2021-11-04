using FluentValidation;

namespace DeveloperHubAPI.V1.Boundary.Request.Validation
{
    public class GetDeveloperHubByIdValidator : AbstractValidator<DeveloperHubQuery>
    {
        public GetDeveloperHubByIdValidator()
        {
            RuleFor(x => x.Id).NotNull()
                              .NotEqual(string.Empty);
        }
    }
}
