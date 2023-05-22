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
                options.AddPolicy("�û����", policy => policy.RequireClaim("�û����"));
                options.AddPolicy("�û��༭", policy => policy.RequireClaim("�û��༭"));
                options.AddPolicy("�û�ɾ��", policy => policy.RequireClaim("�û�ɾ��"));

                options.AddPolicy("��ɫ���", policy => policy.RequireClaim("��ɫ���"));
                options.AddPolicy("��ɫ�༭", policy => policy.RequireClaim("��ɫ�༭"));
                options.AddPolicy("��ɫɾ��", policy => policy.RequireClaim("��ɫɾ��"));

                options.AddPolicy("��Ŀ���", policy => policy.RequireClaim("��Ŀ���"));
                options.AddPolicy("��Ŀ�༭", policy => policy.RequireClaim("��Ŀ�༭"));
                options.AddPolicy("��Ŀɾ��", policy => policy.RequireClaim("��Ŀɾ��"));

                options.AddPolicy("�������", policy => policy.RequireClaim("�������"));
                options.AddPolicy("����༭", policy => policy.RequireClaim("����༭"));
                options.AddPolicy("����ɾ��", policy => policy.RequireClaim("����ɾ��"));

                options.AddPolicy("�������", policy => policy.RequireClaim("�������"));
                options.AddPolicy("���ձ༭", policy => policy.RequireClaim("���ձ༭"));
                options.AddPolicy("����ɾ��", policy => policy.RequireClaim("����ɾ��"));

                options.AddPolicy("ȱ�����", policy => policy.RequireClaim("ȱ�����"));
                options.AddPolicy("ȱ�ݱ༭", policy => policy.RequireClaim("ȱ�ݱ༭"));
                options.AddPolicy("ȱ��ɾ��", policy => policy.RequireClaim("ȱ��ɾ��"));

                options.AddPolicy("֪ͨ���", policy => policy.RequireClaim("֪ͨ���"));

                options.AddPolicy("��Դ���", policy => policy.RequireClaim("��Դ���"));
                options.AddPolicy("��Դʹ��", policy => policy.RequireClaim("��Դʹ��"));
                options.AddPolicy("��Դɾ��", policy => policy.RequireClaim("��Դɾ��"));
                options.AddPolicy("��Դ��¼", policy => policy.RequireClaim("��Դ��¼"));

                options.AddPolicy("����֧��", policy => policy.RequireClaim("����֧��"));
                options.AddPolicy("��������", policy => policy.RequireClaim("��������"));
            });

            //builder.Services.AddScoped<CustomAuthorizationFilter>(provider =>
            //       new CustomAuthorizationFilter(
            //       provider.GetRequiredService<IAuthorizationService>(),
            //       new List<string> { "�û����" } // ��������Ĳ����б�
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