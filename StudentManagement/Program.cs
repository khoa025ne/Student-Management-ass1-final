using Microsoft.EntityFrameworkCore;
using StudentManagement.DAL;
using StudentManagement.DAL.Repositories;
using StudentManagement.BLL.Services;
using StudentManagement.BLL.Services.Interfaces;
using StudentManagement.Middlewares;

namespace StudentManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Lấy chuỗi kết nối từ appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // 2. Đăng ký DbContext với MySQL
            // Đảm bảo bạn đã cài NuGet: Pomelo.EntityFrameworkCore.MySql
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // 3. Đăng ký Repositories (Tầng DAL)
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            // Đăng ký Generic Repository cho các bảng khác (như Role, Class...)
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // 4. Đăng ký Services (Tầng BLL) - Để Controller có thể gọi được
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // 5. Cấu hình Session để lưu thông tin đăng nhập
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Thêm các dịch vụ cho MVC
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Cấu hình Pipeline cho HTTP request
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // --- THỨ TỰ MIDDLEWARE RẤT QUAN TRỌNG ---

            // A. Sử dụng Session trước
            app.UseSession();

            // B. Custom Authentication Middleware (Kiểm tra đăng nhập từ Session)
            app.UseMiddleware<AuthenticationMiddleware>();

            // C. Quyền hạn
            app.UseAuthorization();

            // Cấu hình Route mặc định là trang Login
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}