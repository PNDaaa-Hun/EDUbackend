using EDUBackEnd.Data;
using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Enums;
using EDUBackEnd.Interfaces.Timetable.School;
using EDUBackEnd.Models.Users;
using EDUBackEnd.Services.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EDUBackEnd.Tests.Services
{
    public class GradeServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            var mockSchoolService = new Mock<ICurrentSchoolService>();

            return new AppDbContext(mockSchoolService.Object, options);
        }

        [Fact]
        public async Task AddGradeAsync_ShouldAddGrade_WhenDataIsValid()
        {
            // Arrange
            var context = GetDbContext();
            var service = new GradeService(context);

            var student = new Student { Id = "000000", FirstName = "John", LastName="Doe",SchoolId=1, SchoolClassId = 1  };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            var dto = new AddGradeDto
            {
                StudentId = "000000",
                SubjectId = 1,
                Value = 5,
                GradeType = GradeType.FinalExam
            };

            // Act
            await service.AddGradesAsync(dto);

            // Assert
            var grade = await context.Grades.FirstOrDefaultAsync();
            grade.Should().NotBeNull();
            grade!.Value.Should().Be(5);
            grade.StudentId.Should().Be("000000");
        }

        [Fact]
        public async Task AddGradeAsync_ShouldThrowException_WhenGradeValueIsInvalid()
        {
            // Arrange
            var context = GetDbContext();
            var service = new GradeService(context);
            var dto = new AddGradeDto { Value = 10 }; // Invalid value

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddGradesAsync(dto));
        }

        [Fact]
        public async Task GetGradesById_ShouldReturnGrade_WhenGradeExists()
        {
            // Arrange
            var context = GetDbContext();
            var service = new GradeService(context);
            var grade = new Grade { Id = 10, Value = 4, SubjectId = 1 };
            context.Grades.Add(grade);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetGradesById(10);

            // Assert
            result.Should().NotBeNull();
            result.SubjectId.Should().Be(1);
        }

        [Fact]
        public async Task GetGradesById_ShouldThrowException_WhenGradeDoesNotExist()
        {
            // Arrange
            var context = GetDbContext();
            var service = new GradeService(context);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetGradesById(99));
        }

        [Fact]
        public async Task DeleteGrade_ShouldRemoveGradeFromDatabase()
        {
            // Arrange
            var context = GetDbContext();
            var service = new GradeService(context);
            var grade = new Grade { Id = 1, Value = 3, SubjectId = 1 };
            context.Grades.Add(grade);
            await context.SaveChangesAsync();

            // Act
            await service.DeleteGrades(1);

            // Assert
            var exists = await context.Grades.AnyAsync(g => g.Id == 1);
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateGrade_ShouldUpdateValues_WhenValid()
        {
            // Arrange
            var context = GetDbContext();
            var service = new GradeService(context);
            var grade = new Grade { Id = 1, Value = 3, SubjectId = 1 };
            context.Grades.Add(grade);
            await context.SaveChangesAsync();

            // Detach and modify
            grade.Value = 5;

            // Act
            await service.UpdateGrades(grade);

            // Assert
            var updatedGrade = await context.Grades.FindAsync(1);
            updatedGrade!.Value.Should().Be(5);
        }
    }
}