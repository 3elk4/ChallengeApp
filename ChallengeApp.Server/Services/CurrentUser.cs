using Application.Common.Interfaces;
using System.Security.Claims;

namespace ChallangeApp.Server.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? Name => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}
