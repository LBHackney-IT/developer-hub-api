using System.Collections.Generic;
using System.Linq;
using DeveloperHubAPI.V1.Boundary.Response;
using DeveloperHubAPI.V1.Domain;

namespace DeveloperHubAPI.V1.Factories
{
    public static class ResponseFactory
    {
        public static DeveloperHubResponse ToResponse(this DevelopersHubApi domain)
        {
            return new DeveloperHubResponse()
            {
                Id = domain.Id,
                ApiName = domain.ApiName,
                Description = domain.Description,
                GithubLink = domain.GithubLink,
                SwaggerLink = domain.SwaggerLink,
                StagingBaseURL = domain.StagingBaseURL,
                ApiSpecificationLink = domain.ApiSpecificationLink
            };
        }

        public static List<DeveloperHubResponse> ToResponse(this IEnumerable<DevelopersHubApi> domainList)
        {
            return domainList.Select(domain => domain.ToResponse()).ToList();
        }
        public static ApplicationResponse ToResponse(this Application domain)
        {
            return new ApplicationResponse()
            {
                Name = domain.Name,
                Link = domain.Link,

            };
        }

    }
}
