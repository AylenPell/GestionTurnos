using System.Security.Claims;


namespace Application.Services.Helpers
{
    public static class UserClaimsHelper
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var idValue = user.FindFirst("sub")?.Value
                       ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idValue, out var id) ? id : (int?)null;
        }

        public static string? GetUserDni(this ClaimsPrincipal user)
        {
            return user.FindFirst("dni")?.Value;
        }
    }
}
