using Xunit;
using EDUBackEnd.Services.User;
using System.Linq;
using System.Collections.Generic;

namespace EDUBackEnd.Test.ServiceTests
{
    public class StudentIdGeneratorTests
    {
        [Fact]
        public void GenerateId_ShouldReturnStringOfLengthSix()
        {
            // Act
            string id = StudentIdGenerator.GenerateId();

            // Assert
            Assert.Equal(6, id.Length);
        }

        [Fact]
        public void GenerateId_ShouldOnlyContainAllowedCharacters()
        {
            // Arrange
            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Act
            string id = StudentIdGenerator.GenerateId();

            // Assert
            Assert.All(id, c => Assert.Contains(c, allowedChars));
        }

        [Fact]
        public void GenerateId_ShouldProduceDifferentValues()
        {
            // Arrange
            // Testing randomness is hard, but we can check for collisions in a small batch
            int sampleSize = 100;
            HashSet<string> generatedIds = new HashSet<string>();

            // Act
            for (int i = 0; i < sampleSize; i++)
            {
                generatedIds.Add(StudentIdGenerator.GenerateId());
            }

            // Assert
            // If the generator is working, we should have 100 unique entries
            Assert.Equal(sampleSize, generatedIds.Count);
        }
    }
}