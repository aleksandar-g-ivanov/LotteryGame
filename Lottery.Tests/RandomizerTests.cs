using Lottery.Lib.Utils;

namespace Lottery.Tests
{
    public class RandomizerTests
    {
        [Fact]
        public void GetRandomInRange()
        {
            // Arrange
            var rnd = new RangeRandomizer();
            int from = 5;
            int to = 10;

            // Act
            var results = Enumerable
                .Range(0, 1000)
                .Select(i => rnd.GetRandomInRange(from, to))
                .ToList();

            // Assert
            Assert.All(results, value => Assert.InRange(value, from, to));
        }

        [Fact]
        public void GetRandomInRange_InvalidRange()
        {
            // Arrange
            var rnd = new RangeRandomizer();
            int from = 10;
            int to = 5;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => rnd.GetRandomInRange(from, to));
        }

    }
}
