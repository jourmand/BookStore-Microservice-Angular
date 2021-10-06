using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WebApi.Core.Domain.Commons;

namespace Endpoints.WebApi.Extensions
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            TransactionId = httpContextAccessor?.HttpContext?.TraceIdentifier;
            IpAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            IsAuthenticated = UserId != null;
            UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            IsAdmin = httpContextAccessor?.HttpContext?.User?.IsInRole("admin");
        }

        public string UserId { get; }
        public bool IsAuthenticated { get; }
        public string IpAddress { get; }
        public string TransactionId { get; }

        public string UserName { get; }
        public bool? IsAdmin { get; }
    }
}
