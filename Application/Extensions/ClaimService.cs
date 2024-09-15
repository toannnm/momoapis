using Application.Interfaces.IExtensionServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Extensions
{
    public class ClaimService : IClaimService
    {
        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            var id = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value;
            var username = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var email = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            GetCurrentUserId = string.IsNullOrWhiteSpace(id) ? default : Guid.Parse(id);
            GetCurrentUserName = string.IsNullOrWhiteSpace(username) ? default : username;
            GetCurrentEmail = string.IsNullOrWhiteSpace(email) ? default : email;
            GetCurrentDate = DateTime.UtcNow;
        }

        public Guid? GetCurrentUserId { get; }
        public string? GetCurrentUserName { get; }
        public string? GetCurrentEmail { get; }
        public DateTime? GetCurrentDate { get; }
    }

}
