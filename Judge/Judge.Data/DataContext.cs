using System.Linq;
using Judge.Model.CheckSolution;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data
{
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
            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(o => o.Id);
                builder.HasMany(o => o.UserRoles)
                    .WithOne()
                    .HasForeignKey(o => o.UserId);

                builder.ToTable("Users");
            });

            modelBuilder.Entity<SubmitBase>(builder =>
            {
                builder.HasKey(o => o.Id);
                builder.Property(o => o.Id).UseIdentityColumn();
                builder.Property(o => o.SubmitDateUtc).ValueGeneratedOnAdd();
                builder.Property<byte>("SubmitType");

                builder.HasDiscriminator<byte>(@"SubmitType").HasValue<ProblemSubmit>(1)
                    .HasValue<ContestTaskSubmit>(2);

                builder.HasMany(o => o.Results)
                    .WithOne(o => o.Submit)
                    .HasForeignKey("SubmitId");

                builder.ToTable("Submits", "dbo");
            });

            modelBuilder.Entity<CheckQueue>(builder =>
            {
                builder.HasKey(o => o.SubmitResultId);
                builder.Property(o => o.CreationDateUtc).ValueGeneratedOnAdd();

                builder.ToTable("CheckQueue", "dbo");
            });

            modelBuilder.Entity<SubmitResult>(builder =>
            {
                builder.HasKey(o => o.Id);
                builder.Property(o => o.Id).UseIdentityColumn();

                builder.HasOne(o => o.CheckQueue).WithOne();

                builder.ToTable("SubmitResults", "dbo");
            });

            modelBuilder.Entity<Task>(builder =>
            {
                builder.HasKey(o => o.Id);
                builder.Property(o => o.Id).UseIdentityColumn();
                builder.Property(o => o.CreationDateUtc).ValueGeneratedOnAdd();

                builder.ToTable("Tasks");
            });

            modelBuilder.Entity<Contest>(builder =>
            {
                builder.HasKey(o => o.Id);
                builder.Property(o => o.Id).UseIdentityColumn();
                builder.ToTable("Contests");
            });

            modelBuilder.Entity<ContestTask>(builder =>
            {
                builder.HasKey(o => new {o.ContestId, o.TaskName});

                builder.HasOne(o => o.Task)
                    .WithMany()
                    .HasForeignKey(o => o.TaskId);

                builder.HasOne(o => o.Contest)
                    .WithMany()
                    .HasForeignKey(o => o.ContestId);

                builder.ToTable("ContestTasks");
            });

            modelBuilder.Entity<UserRole>(builder =>
            {
                builder.HasKey(o => o.Id);

                builder.ToTable("UserRoles");
            });
        }

        public CheckQueue DequeueSubmitCheck()
        {
            return this.Set<CheckQueue>().FromSqlRaw("EXEC dbo.DequeueSubmitCheck").AsEnumerable().FirstOrDefault();
        }
    }
}