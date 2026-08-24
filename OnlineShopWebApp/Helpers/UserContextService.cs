using OnlineShopWebApp.Interfaces;
using System.Security.Claims;

namespace OnlineShopWebApp.Helpers 
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentUserID()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context?.User.Identity?.IsAuthenticated == true)
            {
                return context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return null;
        }

        public string? GetAnonymousID()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context?.User.Identity?.IsAuthenticated == true)
            {
                return null;
            }

            return context?.Items["AnonymousID"]?.ToString();
        }
    }
}