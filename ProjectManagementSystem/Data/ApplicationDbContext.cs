using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;
using System.Reflection.Emit;

namespace ProjectManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<MissionDialogue> MissionDialogues { get; set; }
        public DbSet<Risk> Risks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 启用敏感数据记录
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });

            //项目参与人员和用户是多对多
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.Projects)
                .WithMany(p => p.Users)
                .UsingEntity<ProjectUser>();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.PutForwardProjects)
                .WithOne(p => p.PutForward)
                .HasForeignKey(P => P.PutForwardId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.FunctionaryProjects)
                .WithOne(p => p.Functionary)
                .HasForeignKey(P => P.FunctionaryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.PutForwardMissions)
                .WithOne(p => p.PutForward)
                .HasForeignKey(P => P.PutForwardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.ExecuteMissions)
                .WithMany(p => p.Executors)
                .UsingEntity<MissionExecutor>();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.PutForwardDefects)
                .WithOne(p => p.PutForward)
                .HasForeignKey(P => P.PutForwardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.FunctionaryDefects)
                .WithOne(p => p.Functionary)
                .HasForeignKey(P => P.FunctionaryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.PutForwardRisks)
                .WithOne(p => p.PutForward)
                .HasForeignKey(P => P.PutForwardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.FunctionaryRisks)
                .WithOne(p => p.Functionary)
                .HasForeignKey(P => P.FunctionaryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.MissionDialogues)
                .WithOne(p => p.Speaker)
                .HasForeignKey(P => P.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany<Mission>()
                .WithOne(m => m.Project)
                .HasForeignKey(m => m.MissionProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany<Defect>()
                .WithOne(d => d.Project)
                .HasForeignKey(d => d.DefectProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany<Risk>()
                .WithOne(r => r.Project)
                .HasForeignKey(r => r.RiskProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.PutForward)
                .WithMany(u => u.PutForwardProjects)
                .HasForeignKey(p => p.PutForwardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Functionary)
                .WithMany(u => u.FunctionaryProjects)
                .HasForeignKey(p => p.FunctionaryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
