﻿// <auto-generated />
using System;
using Judge.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Judge.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Judge.Model.CheckSolution.Task", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDateUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsOpened")
                        .HasColumnType("bit");

                    b.Property<int>("MemoryLimitBytes")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Statement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TestsFolder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimeLimitMilliseconds")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Judge.Model.Contests.Contest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CheckPointTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FreezeTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsOpened")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OneLanguagePerTask")
                        .HasColumnType("bit");

                    b.Property<int>("Rules")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Contests");
                });

            modelBuilder.Entity("Judge.Model.Contests.ContestTask", b =>
                {
                    b.Property<int>("ContestId")
                        .HasColumnType("int");

                    b.Property<string>("TaskName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("TaskId")
                        .HasColumnType("bigint");

                    b.HasKey("ContestId", "TaskName");

                    b.HasIndex("TaskId");

                    b.ToTable("ContestTasks");
                });

            modelBuilder.Entity("Judge.Model.Entities.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompilerOptionsTemplate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompilerPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCompilable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutputFileTemplate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RunStringFormat")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Judge.Model.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Judge.Model.Entities.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Judge.Model.SubmitSolution.CheckQueue", b =>
                {
                    b.Property<long>("SubmitResultId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDateUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.HasKey("SubmitResultId");

                    b.ToTable("CheckQueue","dbo");
                });

            modelBuilder.Entity("Judge.Model.SubmitSolution.SubmitBase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<long>("ProblemId")
                        .HasColumnType("bigint");

                    b.Property<string>("SessionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmitDateUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<byte>("SubmitType")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserHost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Submits","dbo");

                    b.HasDiscriminator<byte>("SubmitType");
                });

            modelBuilder.Entity("Judge.Model.SubmitSolution.SubmitResult", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompileOutput")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PassedTests")
                        .HasColumnType("int");

                    b.Property<string>("RunDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RunOutput")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long?>("SubmitId")
                        .HasColumnType("bigint");

                    b.Property<int?>("TotalBytes")
                        .HasColumnType("int");

                    b.Property<int?>("TotalMilliseconds")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubmitId");

                    b.ToTable("SubmitResults","dbo");
                });

            modelBuilder.Entity("Judge.Model.SubmitSolution.ContestTaskSubmit", b =>
                {
                    b.HasBaseType("Judge.Model.SubmitSolution.SubmitBase");

                    b.Property<int>("ContestId")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue((byte)2);
                });

            modelBuilder.Entity("Judge.Model.SubmitSolution.ProblemSubmit", b =>
                {
                    b.HasBaseType("Judge.Model.SubmitSolution.SubmitBase");

                    b.HasDiscriminator().HasValue((byte)1);
                });

            modelBuilder.Entity("Judge.Model.Contests.ContestTask", b =>
                {
                    b.HasOne("Judge.Model.Contests.Contest", "Contest")
                        .WithMany()
                        .HasForeignKey("ContestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Judge.Model.CheckSolution.Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Judge.Model.Entities.UserRole", b =>
                {
                    b.HasOne("Judge.Model.Entities.User", null)
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Judge.Model.SubmitSolution.CheckQueue", b =>
                {
                    b.HasOne("Judge.Model.SubmitSolution.SubmitResult", null)
                        .WithOne("CheckQueue")
                        .HasForeignKey("Judge.Model.SubmitSolution.CheckQueue", "SubmitResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Judge.Model.SubmitSolution.SubmitResult", b =>
                {
                    b.HasOne("Judge.Model.SubmitSolution.SubmitBase", "Submit")
                        .WithMany("Results")
                        .HasForeignKey("SubmitId");
                });
#pragma warning restore 612, 618
        }
    }
}