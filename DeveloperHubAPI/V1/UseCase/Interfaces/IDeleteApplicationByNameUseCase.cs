using DeveloperHubAPI.V1.Boundary.Request;
using DeveloperHubAPI.V1.Boundary.Response;
using System.Threading.Tasks;

namespace DeveloperHubAPI.V1.UseCase.Interfaces
{
    public interface IDeleteApplicationByNameUseCase
    {
        Task<ApplicationResponse> Execute(DeleteApplicationByNameRequest query);
    }
}