using EDUBackEnd.Interfaces.Timetable;
using EDUBackEnd.Interfaces.Timetable.School;
using EDUBackEnd.Models.BookDomain;
using EDUBackEnd.Models.Chat;
using EDUBackEnd.Models.Notificaiton;
using EDUBackEnd.Models.Timetable;
using EDUBackEnd.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Reflection;

namespace EDUBackEnd.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        private readonly ICurrentSchoolService _currentSchoolService;
        public int CurrentSchoolId { get; private set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<School> Schools { get; set; }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<TeachingRequirement> TeachingRequirements { get; set; }
        public DbSet<ScheduleEntry> ScheduleEntries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Models.Attachment.Attachments> Attachments => Set<Models.Attachment.Attachments>();
        public DbSet<Message> Messages { get; set; }
        public AppDbContext(ICurrentSchoolService currentSchoolService,
            DbContextOptions options) : base(options)
        {
            _currentSchoolService = currentSchoolService;
            CurrentSchoolId = _currentSchoolService?.SchoolId ?? 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId);

            modelBuilder.Entity<ScheduleEntry>()
                .HasIndex(e => new { e.TimeSlotId, e.TeacherId })
                .IsUnique();

            modelBuilder.Entity<ScheduleEntry>()
                .HasIndex(e => new { e.TimeSlotId, e.SchoolClassId })
                .IsUnique();

            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = "Admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "Teacher",
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                },
                new IdentityRole
                {
                    Id = "Student",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                },
                new IdentityRole
                {
                    Id = "TimeTableManager",
                    Name = "TimeTableManager",
                    NormalizedName = "TIMETABLEMANAGER"
                },
                new IdentityRole
                {
                    Id = "Principal",
                    Name = "Principal",
                    NormalizedName = "PRINCIPAL"
                },
                new IdentityRole
                {
                    Id = "PrincipalAssistant",
                    Name = "PrincipalAssistant",
                    NormalizedName = "PRINCIPALASSISTANT"
                },
                new IdentityRole
                {
                    Id = "HeadTeacher",
                    Name = "HeadTeacher",
                    NormalizedName = "HEADTEACHER"
                },
                new IdentityRole
                {
                    Id = "Counselor",
                    Name = "Counselor",
                    NormalizedName = "COUNSELOR"
                },
                new IdentityRole
                {
                    Id = "Librarian",
                    Name = "Librarian",
                    NormalizedName = "LIBRARIAN"
                },
                new IdentityRole
                {
                    Id = "Nurse",
                    Name = "Nurse",
                    NormalizedName = "NURSE"
                },
                new IdentityRole
                {
                    Id = "Janitor",
                    Name = "Janitor",
                    NormalizedName = "JANITOR"
                },
                new IdentityRole
                {
                    Id = "CafeteriaStaff",
                    Name = "CafeteriaStaff",
                    NormalizedName = "CAFETERIASTAFF"
                },
                new IdentityRole
                {
                    Id = "ITSupport",
                    Name = "ITSupport",
                    NormalizedName = "ITSUPPORT"
                }


            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
            base.OnModelCreating(modelBuilder);

            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISchoolEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetGlobalFilter),
                        BindingFlags.NonPublic | BindingFlags.Instance)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(this, new object[] { modelBuilder });

                }
            }
        }
        private void SetGlobalFilter<TEntity>(ModelBuilder builder)
        where TEntity : class, ISchoolEntity
        {
            builder.Entity<TEntity>()
                .HasQueryFilter(e =>
                    CurrentSchoolId == 0 || e.SchoolId == CurrentSchoolId);
        }
    }

}
