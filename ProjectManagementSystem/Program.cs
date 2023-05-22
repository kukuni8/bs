using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Hubs;
using ProjectManagementSystem.Models;
using System.Security.Principal;
using System;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementSystem.Filter;

namespace ProjectManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IImageService, ImageHelper>();

            builder.Services.AddScoped<UserManager<ApplicationUser>>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ProjectManagementSystem")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.User.AllowedUserNameCharacters = null;

            })
            .AddRoleManager<RoleManager<IdentityRole<int>>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("用户添加", policy => policy.RequireClaim("用户添加"));
                options.AddPolicy("用户编辑", policy => policy.RequireClaim("用户编辑"));
                options.AddPolicy("用户删除", policy => policy.RequireClaim("用户删除"));

                options.AddPolicy("角色添加", policy => policy.RequireClaim("角色添加"));
                options.AddPolicy("角色编辑", policy => policy.RequireClaim("角色编辑"));
                options.AddPolicy("角色删除", policy => policy.RequireClaim("角色删除"));

                options.AddPolicy("项目添加", policy => policy.RequireClaim("项目添加"));
                options.AddPolicy("项目编辑", policy => policy.RequireClaim("项目编辑"));
                options.AddPolicy("项目删除", policy => policy.RequireClaim("项目删除"));

                options.AddPolicy("任务添加", policy => policy.RequireClaim("任务添加"));
                options.AddPolicy("任务编辑", policy => policy.RequireClaim("任务编辑"));
                options.AddPolicy("任务删除", policy => policy.RequireClaim("任务删除"));

                options.AddPolicy("风险添加", policy => policy.RequireClaim("风险添加"));
                options.AddPolicy("风险编辑", policy => policy.RequireClaim("风险编辑"));
                options.AddPolicy("风险删除", policy => policy.RequireClaim("风险删除"));

                options.AddPolicy("缺陷添加", policy => policy.RequireClaim("缺陷添加"));
                options.AddPolicy("缺陷编辑", policy => policy.RequireClaim("缺陷编辑"));
                options.AddPolicy("缺陷删除", policy => policy.RequireClaim("缺陷删除"));

                options.AddPolicy("通知添加", policy => policy.RequireClaim("通知添加"));

                options.AddPolicy("资源添加", policy => policy.RequireClaim("资源添加"));
                options.AddPolicy("资源使用", policy => policy.RequireClaim("资源使用"));
                options.AddPolicy("资源删除", policy => policy.RequireClaim("资源删除"));
                options.AddPolicy("资源记录", policy => policy.RequireClaim("资源记录"));

                options.AddPolicy("新增支出", policy => policy.RequireClaim("新增支出"));
                options.AddPolicy("新增收入", policy => policy.RequireClaim("新增收入"));
            });

            //builder.Services.AddScoped<CustomAuthorizationFilter>(provider =>
            //       new CustomAuthorizationFilter(
            //       provider.GetRequiredService<IAuthorizationService>(),
            //       new List<string> { "用户添加" } // 这里是你的策略列表
            //      )
            //     );
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
            });


            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapHub<NotificationHub>("/noticeHub");
            });

            app.Run();
        }
    }
}