using EDUBackEnd.Data;
using EDUBackEnd.Interfaces.Timetable.School;
using EDUBackEnd.Models.Users;
using EDUBackEnd.Services.User;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EDUBackEnd.Test.ServiceTests
{
    public class AdminServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockSchoolService = new Mock<ICurrentSchoolService>();


            return new AppDbContext(mockSchoolService.Object, options);
        }

        private Mock<UserManager<User>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userValidators = new List<IUserValidator<User>>();
            var passwordValidators = new List<IPasswordValidator<User>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new IdentityErrorDescriber();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<User>>>();

            return new Mock<UserManager<User>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errors,
                services.Object,
                logger.Object
            );
        }
        private AdminService GetService(AppDbContext context,
            Mock<UserManager<User>> userManagerMock)
            => new AdminService(context, userManagerMock.Object);


        // STUDENT TESTS

        [Fact]
        public async Task CreateStudentAsync_ShouldAddStudent()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            var student = new Student
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Middle",
                SchoolId = 1,
                SchoolClassId = 1,
            };

            var result = await service.CreateStudentAsync(student);

            result.Id.Should().NotBeNullOrEmpty();

            var dbStudent = await context.Students.FirstOrDefaultAsync();
            dbStudent.Should().NotBeNull();
            dbStudent!.FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetStudentByIdAsync_ShouldReturnStudent()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            var student = await service.CreateStudentAsync(new Student
            {
                FirstName = "Jane",
                LastName = "Doe",
                MiddleName = "Middle",
                SchoolId = 1,
                SchoolClassId = 1
            });

            var result = await service.GetStudentByIdAsync(student.Id);

            result.Should().NotBeNull();
            result.FirstName.Should().Be("Jane");
        }

        [Fact]
        public async Task GetStudentByIdAsync_ShouldThrow_WhenNotFound()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            await Assert.ThrowsAsync<Exception>(() =>
                service.GetStudentByIdAsync("invalid-id"));
        }

        [Fact]
        public async Task UpdateStudentAsync_ShouldUpdateStudent()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            var student = await service.CreateStudentAsync(new Student
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Middle",
                SchoolId = 1,
                SchoolClassId = 1
            });

            student.FirstName = "New";
            await service.UpdateStudentAsync(student);

            var updated = await context.Students.FirstAsync();
            updated.FirstName.Should().Be("New");
        }

        [Fact]
        public async Task DeleteStudentAsync_ShouldRemoveStudent_And_DeleteUser()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            var student = await service.CreateStudentAsync(new Student
            {
                Id = "AAAAAA",
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Middle",
                SchoolId = 1,
                SchoolClassId = 1
            });
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                StudentId = student.Id,
                ClassId = student.SchoolClassId,
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            userManagerMock
                .Setup(x => x.Users)
                .Returns(context.Users);

            userManagerMock
                .Setup(x => x.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            await service.DeleteStudentAsync(student.Id);

            context.Students.Count().Should().Be(0);

            userManagerMock
                .Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once);
        }


        // ADMIN TESTS


        [Fact]
        public async Task CreateAdminAsync_ShouldAddAdmin()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            var admin = new Admin
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Middle",
                SchoolId = 1,
            };

            await service.CreateAdminAsync(admin);

            context.Admins.Count().Should().Be(1);
        }

        [Fact]
        public async Task DeleteAdminAsync_ShouldRemoveAdmin()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            var admin = new Admin
            {
                Id = 102,
                FirstName = "ToDelete",
                LastName = "Admin",
                SchoolId = 1,
                MiddleName = "Middle"
            };

            await service.CreateAdminAsync(admin);

            await service.DeleteAdminAsync(102);

            context.Admins.Count().Should().Be(0);
        }


        // TEACHER TESTS


        [Fact]
        public async Task CreateTeacherAsync_ShouldAddTeacher()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            var teacher = new Teacher
            {
                FirstName = "Teacher",
                LastName = "One",
                SchoolId = 1,
                MiddleName = "Middle"
            };

            await service.CreateTeacherAsync(teacher);

            context.Teachers.Count().Should().Be(1);
        }

        // SCHOOL TESTS
        

        [Fact]
        public async Task CreateSchoolAsync_ShouldAddSchool()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            var school = new School
            {
                Name = "Test School",
                Address = "Test Street 1",
                City = "Test City",
                Country = "Test Country",
                PostalCode = "12345",
                Region = "Test Region"
            };

            await service.CreateSchoolAsync(school);

            context.Schools.Count().Should().Be(1);
        }

        // ============================
        // GET ALL TEST
        // ============================

        [Fact]
        public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
        {
            var context = GetInMemoryDbContext();
            var userManagerMock = GetMockUserManager();
            var service = GetService(context, userManagerMock);

            await service.CreateStudentAsync(new Student { FirstName = "A" , LastName = "A", SchoolId = 1, SchoolClassId = 1 });
            await service.CreateStudentAsync(new Student {FirstName = "B", LastName = "B", SchoolId = 1, SchoolClassId = 1 });

            var result = await service.GetStudentsAsync();

            result.Count.Should().Be(2);
        }
    }
}