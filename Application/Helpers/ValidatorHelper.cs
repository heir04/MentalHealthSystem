using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MentalHealthSystem.Application.Interfaces.IRepositories;

namespace MentalHealthSystem.Application.Helpers
{
    public class ValidatorHelper(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public Guid GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Guid.Empty;
            }

            return userId;
        }

        public string GetUserRole()
        {
            var userRoleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            return userRoleClaim ?? string.Empty;
        }
    }
}