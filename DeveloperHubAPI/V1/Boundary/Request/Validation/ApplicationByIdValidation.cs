using FluentValidation;

namespace DeveloperHubAPI.V1.Boundary.Request.Validation
{
    public class ApplicationByIdValidation : AbstractValidator<ApplicationByIdRequest>
    {
        public ApplicationByIdValidation()
        {
            RuleFor(x => x.Id).NotNull()
                              .NotEqual(string.Empty);
        }
    }
}
