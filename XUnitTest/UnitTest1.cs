using FluentAssertions;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestWithoutFluentAssertions()
        {
            // Arrange
            int x = 250, y = 10, z;

            //Act
            z = x + y;
            // Assert
            Assert.Equal((x + y), z);
        }

        [Fact]
        public void TestWithFluentAssertions()
        {
            // Arrange
            int x = 250, y = 10, z;

            //Act
            z = x + y;
            // Assert
            z.Should().Be(x + y, "Values do not match");
        }

        [Fact]
        public void StringShouldStartWith()
        {
            // Arrange
            string s = "Hello, World!";
            //Act
            // Assert
            s.Should().StartWith("Hello");
        }

        [Fact]
        public void StringShouldStartWithHello_AndLength_13_And_endWith_ExclamationMark()
        {
            // Arrange
            string s = "Hello, World!";
            //Act
            bool Failed = false;
            // Assert
            s.Should().StartWith("Hello")
                .And.HaveLength(13)
                .And.EndWith("!")
                .And.NotBeNullOrWhiteSpace()
                .And.BeOfType<string>();

            Failed.Should().BeFalse("The string should meet all the specified conditions.");
        }

    }
}
