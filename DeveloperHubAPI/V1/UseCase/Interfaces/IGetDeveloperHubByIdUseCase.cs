using DeveloperHubAPI.V1.Boundary.Response;

namespace DeveloperHubAPI.V1.UseCase.Interfaces
{
    public interface IGetDeveloperHubByIdUseCase
    {
        DeveloperHubResponse Execute(DeveloperHubQuery developerHubQuery);
    }
}
