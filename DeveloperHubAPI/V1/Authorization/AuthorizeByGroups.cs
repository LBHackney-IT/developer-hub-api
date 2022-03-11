using System;
using System.Linq;
using Hackney.Core.Http;
using Hackney.Core.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeveloperHubAPI.V1.Authorization
{
    // TODO: Write automated tests & possibly move to shared package once implemented
    public class AuthorizeByGroups : TypeFilterAttribute
    {
        /// <summary>
        /// Authorise this endpoint using permitted groups
        /// </summary>
        /// <param name="permittedGroupsVariable">
        /// The name of the environment variable that stores the permitted groups for the endpoint
        /// </param>
        public AuthorizeByGroups(string permittedGroupsVariable)
            : base(typeof(TokenGroupsFilter))
        {
            Arguments = new object[] { permittedGroupsVariable };
        }
    }

    public class TokenGroupsFilter : IAuthorizationFilter
    {
        private readonly string[] _requiredGoogleGroups;
        private readonly ITokenFactory _tokenFactory;
        private readonly IHttpContextWrapper _contextWrapper;

        public TokenGroupsFilter(IHttpContextWrapper contextWrapper, ITokenFactory tokenFactory, string permittedGroupsVariable)
        {
            _contextWrapper = contextWrapper;
            _tokenFactory = tokenFactory;

            var requiredGooglepermittedGroupsVariable = Environment.GetEnvironmentVariable(permittedGroupsVariable);
            if (requiredGooglepermittedGroupsVariable is null) throw new Exception($"Cannot resolve {permittedGroupsVariable} environment variable!");

            _requiredGoogleGroups = requiredGooglepermittedGroupsVariable.Split(','); // Note: Env variable must not have spaces after commas
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = _tokenFactory.Create(_contextWrapper.GetContextRequestHeaders(context.HttpContext));
            if (token is null || !token.Groups.Any(g => _requiredGoogleGroups.Contains(g)))
            {
                context.Result = new UnauthorizedObjectResult($"User {token?.Name} is not authorized to access this endpoint");
            }
        }
    }
}
