using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }


        public DbSet<Defect> Defects { get; set; }
        public DbSet<DefectType> DefectTypes { get; set; }
        public DbSet<DefectStatus> DefectStatuses { get; set; }



        public DbSet<Mission> Missions { get; set; }
        public DbSet<MissionStatus> MissionStatuses { get; set; }
        public DbSet<MissionPriority> MissionPriority { get; set; }
        public DbSet<MissionDialogue> MissionDialogues { get; set; }
        public DbSet<MissionType> MissionTypes { get; set; }


        public DbSet<Risk> Risks { get; set; }
        public DbSet<RiskLevel> RiskLevels { get; set; }
        public DbSet<RiskStatus> RiskStatuses { get; set; }
        public DbSet<RiskType> RiskTypes { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
