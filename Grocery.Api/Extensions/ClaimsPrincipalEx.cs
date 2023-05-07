using System.Security.Claims;

namespace Grocery.Api.Extensions
{
    public static class ClaimsPrincipalEx
    {
        public static int GetId(this ClaimsPrincipal identy)
        {
            var userId = int.Parse(identy.Claims
                .FirstOrDefault(c => c.Type == "Id")?
                .Value);

            return userId;
        }
    }
}