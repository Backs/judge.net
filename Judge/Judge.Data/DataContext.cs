using System.Linq;
using Judge.Model.CheckSolution;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data;

internal sealed class DataContext : DbContext
{
    private readonly string connectionString;

    public DataContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(this.connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Language>(builder =>
        {
            builder.HasKey(o => o.Id);
            builder.ToTable("Languages");
            builder.Property(o => o.CompilerPath).HasMaxLength(1024);
            builder.Property(o => o.Description).HasMaxLength(1024);
            builder.Property(o => o.Name).HasMaxLength(128);
            builder.Property(o => o.CompilerOptionsTemplate).HasMaxLength(512);
            builder.Property(o => o.OutputFileTemplate).HasMaxLength(512);
            builder.Property(o => o.RunStringFormat).HasMaxLength(512);
        });

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(o => o.Id);
            builder.HasMany(o => o.UserRoles)
                .WithOne()
                .HasForeignKey(o => o.UserId);
            builder.Property(o => o.Email).HasMaxLength(256);
            builder.Property(o => o.PasswordHash).HasMaxLength(256);
            builder.Property(o => o.UserName).HasMaxLength(256);

            builder.ToTable("Users");
        });

        modelBuilder.Entity<SubmitBase>(builder =>
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.SubmitDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .HasColumnType("datetime");
            builder.Property<byte>("SubmitType");

            builder.HasDiscriminator<byte>("SubmitType")
                .HasValue<ProblemSubmit>(1)
                .HasValue<ContestTaskSubmit>(2);

            builder.HasMany(o => o.Results)
                .WithOne(o => o.Submit)
                .HasForeignKey("SubmitId");

            builder.ToTable("Submits", "dbo");

            builder.Property(o => o.FileName).HasMaxLength(256);
            builder.Property(o => o.UserHost).HasMaxLength(64);
            builder.Property(o => o.SessionId).HasMaxLength(32);

            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);
        });

        modelBuilder.Entity<CheckQueue>(builder =>
        {
            builder.HasKey(o => o.SubmitResultId);
            builder.Property(o => o.CreationDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .HasColumnType("datetime");

            builder.ToTable("CheckQueue", "dbo");
        });

        modelBuilder.Entity<SubmitResult>(builder =>
        {
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.CheckQueue).WithOne();
            builder.Property(o => o.RunDescription).HasMaxLength(4096);
            builder.Property(o => o.RunOutput).HasMaxLength(4096);

            builder.ToTable("SubmitResults", "dbo");
        });

        modelBuilder.Entity<Task>(builder =>
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.CreationDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .HasColumnType("datetime");
            builder.Property(o => o.TestsFolder).HasMaxLength(512);
            builder.Property(o => o.Name).HasMaxLength(256);

            builder.ToTable("Tasks");
        });

        modelBuilder.Entity<Contest>(builder =>
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Name).HasMaxLength(128);
            builder.Property(o => o.FinishTime).HasColumnType("datetime");
            builder.Property(o => o.FreezeTime).HasColumnType("datetime");
            builder.Property(o => o.CheckPointTime).HasColumnType("datetime");
            builder.Property(o => o.StartTime).HasColumnType("datetime");
            builder.ToTable("Contests");
        });

        modelBuilder.Entity<ContestTask>(builder =>
        {
            builder.HasKey(o => new { o.ContestId, o.TaskName });

            builder.HasOne(o => o.Task)
                .WithMany()
                .HasForeignKey(o => o.TaskId);

            builder.HasOne(o => o.Contest)
                .WithMany()
                .HasForeignKey(o => o.ContestId);

            builder.Property(o => o.TaskName).HasMaxLength(5);

            builder.ToTable("ContestTasks");
        });

        modelBuilder.Entity<UserRole>(builder =>
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.RoleName).HasMaxLength(32);

            builder.ToTable("UserRoles");
        });
    }

    public CheckQueue? DequeueSubmitCheck()
    {
        return this.Set<CheckQueue>().FromSqlRaw("EXEC dbo.DequeueSubmitCheck").AsEnumerable().FirstOrDefault();
    }
}