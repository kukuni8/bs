using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Models;


namespace ProjectManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<MissionDialogue> MissionDialogues { get; set; }
        public DbSet<Risk> Risks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<MissionExecutor> MissionExecutors { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceChange> ResourceChanges { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<FundChange> FundChanges { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<NoticeReceiver> NoticeReceivers { get; set; }
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

            modelBuilder.Entity<Project>()
             .HasOne(p => p.Fund)
             .WithOne(f => f.Project)
             .HasForeignKey<Fund>(f => f.ProjectId);


            modelBuilder.Entity<FundChange>()
             .Property(f => f.Number)
             .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.ApplicationUserId });

            modelBuilder.Entity<NoticeReceiver>()
                .HasKey(nr => new { nr.NoticeId, nr.ReceiverId });

            modelBuilder.Entity<Notice>()
                .HasMany(n => n.Receivers)
                .WithOne(nr => nr.Notice)
                .HasForeignKey(nr => nr.NoticeId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.NoticeReceived)
                .WithOne(nr => nr.Receiver)
                 .HasForeignKey(nr => nr.ReceiverId);

            modelBuilder.Entity<Project>()
            .HasMany(p => p.ProjectUsers)
            .WithOne(pu => pu.Project)
            .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ProjectUsers)
                .WithOne(pu => pu.ApplicationUser)
                .HasForeignKey(pu => pu.ApplicationUserId);



            modelBuilder.Entity<MissionExecutor>()
                .HasKey(me => new { me.ApplicationUserId, me.MissionId });

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.MissionExecutors)
                .WithOne(me => me.ApplicationUser)
                .HasForeignKey(me => me.ApplicationUserId);

            modelBuilder.Entity<Mission>()
                .HasMany(u => u.MissionExecutors)
                .WithOne(me => me.Mission)
                .HasForeignKey(me => me.MissionId);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });


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
                .HasOne(p => p.PutForward)
                .WithMany(u => u.PutForwardProjects)
                .HasForeignKey(p => p.PutForwardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Functionary)
                .WithMany(u => u.FunctionaryProjects)
                .HasForeignKey(p => p.FunctionaryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatRecord>()
           .HasOne(cr => cr.Putforward)
           .WithMany(u => u.PutforwardChats)
           .HasForeignKey(cr => cr.PutforwardId)
            .OnDelete(DeleteBehavior.Restrict); // 禁用级联删除

            modelBuilder.Entity<ChatRecord>()
                .HasOne(cr => cr.Receiver)
                .WithMany(u => u.ReceiveChats)
                .HasForeignKey(cr => cr.ReceiverId)
                 .OnDelete(DeleteBehavior.Restrict); // 禁用级联删除
        }
    }
}
