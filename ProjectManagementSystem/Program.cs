using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ProjectManagementSystem")));

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 1;
				//options.SignIn.RequireConfirmedAccount = true;
			})
		   .AddSignInManager<SignInManager<ApplicationUser>>()
		   .AddDefaultTokenProviders()
		   .AddEntityFrameworkStores<ApplicationDbContext>();



			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("�û�����", policy => policy.RequireClaim("�û�����"));
				options.AddPolicy("��ɫ����", policy => policy.RequireClaim("��ɫ����"));
				options.AddPolicy("��Ŀ����", policy => policy.RequireClaim("��Ŀ����"));
				options.AddPolicy("�������", policy => policy.RequireClaim("�������"));
				options.AddPolicy("���չ���", policy => policy.RequireClaim("���չ���"));
				options.AddPolicy("ȱ�ݹ���", policy => policy.RequireClaim("ȱ�ݹ���"));
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


			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Account}/{action=Login}/{id?}");

			app.Run();
		}
	}
}