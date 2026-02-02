using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace StudentManagement.Middlewares
{
    /// <summary>
    /// Middleware kiểm tra authentication và redirect về Login
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";
            
            // Các route không cần authentication
            var publicRoutes = new[]
            {
                "/",
                "/home",
                "/home/index",
                "/auth/login",
                "/auth/register", 
                "/auth/logout",
                "/css/",
                "/js/",
                "/lib/",
                "/favicon.ico"
            };

            // Kiểm tra xem có phải route public không
            bool isPublicRoute = publicRoutes.Any(route => path.StartsWith(route));

            if (!isPublicRoute)
            {
                // Kiểm tra authentication từ Session
                var userId = context.Session.GetString("UserId");
                var userRole = context.Session.GetString("UserRole");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
                {
                    // Chưa đăng nhập -> redirect về Login
                    context.Response.Redirect("/Auth/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}