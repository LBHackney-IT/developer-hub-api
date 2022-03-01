using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Domain;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.UseCase.Interfaces
{
    public interface IUpdateApplicationUseCase
    {
        Task<DevelopersHubApi> Execute(ApplicationByNameRequest pathParameters, UpdateApplicationListItem bodyParameters);
    }
}
