using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extentions
{
    public static class IdentityExtensions
    {
        public static string? GetUsername(this ClaimsPrincipal claimsPrincipal)
        {
            //return claimsPrincipal.Claims.FirstOrDefault(u=>u.Type== ClaimTypes.Name)?.Value;
            //return claimsPrincipal.Identity?.Name ?? "";

            return claimsPrincipal.Identity?.Name;
        }

        public static int? GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return default;
            return int.Parse(userId);
        }


        public static string ? GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(u=>u.Type==ClaimTypes.Email)?.Value;
        }
    }
}
