using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Cater4Us_Backend.Services
{
    public class AdminOrCustomTokenHandler : AuthorizationHandler<AdminOnlyRequirement>
    {
        private readonly IMemoryCache _memoryCache;

        public AdminOrCustomTokenHandler(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOnlyRequirement requirement)
        {
            var userRole = context.User.FindFirstValue(ClaimTypes.Role);
            if (userRole == "1")
            {
                // Allow if the user has the admin role (role "1")
                context.Succeed(requirement);
            }
            else
            {
                // Check if the provided token matches the one stored in memory
                if (context.Resource is Microsoft.AspNetCore.Http.HttpContext httpContext)
                {
                    string token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                    if (_memoryCache.TryGetValue("AdminToken", out string storedToken) && storedToken == token)
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
public class AdminOnlyRequirement : IAuthorizationRequirement
{
    // Custom properties or methods can be added if needed
}
