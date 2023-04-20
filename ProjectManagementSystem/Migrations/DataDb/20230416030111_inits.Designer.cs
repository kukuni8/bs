﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectManagementSystem.Data;

#nullable disable

namespace ProjectManagementSystem.Migrations.DataDb
{
    [DbContext(typeof(DataDbContext))]
    [Migration("20230416030111_inits")]
    partial class inits
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProjectManagementSystem.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("IdCardNo")
                        .HasMaxLength(18)
                        .HasColumnType("nvarchar(18)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("MissionId")
                        .HasColumnType("int");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MissionId");

                    b.ToTable("ApplicationUser");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Defect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DefectStatusId")
                        .HasColumnType("int");

                    b.Property<int?>("DefectTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Solution")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DefectStatusId");

                    b.HasIndex("DefectTypeId");

                    b.ToTable("Defects");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.DefectStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("StatusName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DefectStatuses");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.DefectType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("TypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DefectTypes");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Mission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PriorityId")
                        .HasColumnType("int");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PriorityId");

                    b.HasIndex("StatusId");

                    b.ToTable("Missions");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.MissionPriority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Priority")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MissionPriority");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.MissionStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MissionStatuses");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Budget")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PutForward")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SelectFunctionary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.ProjectStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProjectStatuses");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Risk", b =>
                {
                    b.Property<int>("RiskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RiskId"), 1L, 1);

                    b.Property<DateTime>("RiskCreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RiskDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RiskIncidence")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RiskLevelId")
                        .HasColumnType("int");

                    b.Property<string>("RiskName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RiskProbability")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RiskSolution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RiskStatusId")
                        .HasColumnType("int");

                    b.Property<string>("RiskType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RiskId");

                    b.HasIndex("RiskLevelId");

                    b.HasIndex("RiskStatusId");

                    b.ToTable("Risks");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.RiskLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("LevelName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RiskLevels");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.RiskStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("StatusName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RiskStatuses");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.ApplicationUser", b =>
                {
                    b.HasOne("ProjectManagementSystem.Models.Mission", null)
                        .WithMany("applicationUsers")
                        .HasForeignKey("MissionId");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Defect", b =>
                {
                    b.HasOne("ProjectManagementSystem.Models.DefectStatus", "DefectStatus")
                        .WithMany()
                        .HasForeignKey("DefectStatusId");

                    b.HasOne("ProjectManagementSystem.Models.DefectType", "DefectType")
                        .WithMany()
                        .HasForeignKey("DefectTypeId");

                    b.Navigation("DefectStatus");

                    b.Navigation("DefectType");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Mission", b =>
                {
                    b.HasOne("ProjectManagementSystem.Models.MissionPriority", "Priority")
                        .WithMany()
                        .HasForeignKey("PriorityId");

                    b.HasOne("ProjectManagementSystem.Models.MissionStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.Navigation("Priority");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Risk", b =>
                {
                    b.HasOne("ProjectManagementSystem.Models.RiskLevel", "RiskLevel")
                        .WithMany()
                        .HasForeignKey("RiskLevelId");

                    b.HasOne("ProjectManagementSystem.Models.RiskStatus", "RiskStatus")
                        .WithMany()
                        .HasForeignKey("RiskStatusId");

                    b.Navigation("RiskLevel");

                    b.Navigation("RiskStatus");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Mission", b =>
                {
                    b.Navigation("applicationUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
