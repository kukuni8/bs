using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data
{
    public class DataDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Mission> Missions { get; set; }

        public DbSet<Defect> Defects { get; set; }
        public DbSet<Risk> Risks { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<MissionStatus> MissionStatuses { get; set; }
        public DbSet<MissionPriority> MissionPriority { get; set; }

        public DbSet<DefectType> DefectTypes { get; set; }
        public DbSet<DefectStatus> DefectStatuses { get; set; }
        public DbSet<RiskLevel> RiskLevels { get; set; }
        public DbSet<RiskStatus> RiskStatuses { get; set; }
        public DbSet<RiskType> RiskTypes { get; set; }
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //需要填写连接字符串和数据库版本
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProjectManagementSystem;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
