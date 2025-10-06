using BookStackMcpServer.Models;

namespace BookStackMcpServer.Tests.Models;

public class CachingOptionsTests
{
    [Fact]
    public void SectionName_ReturnsCorrectValue()
    {
        // Assert
        Assert.Equal("Caching", CachingOptions.SectionName);
    }

    [Fact]
    public void Properties_CanBeSet()
    {
        // Arrange
        var options = new CachingOptions
        {
            Enabled = false,
            AbsoluteExpirationMinutes = 10,
            SlidingExpirationMinutes = 3
        };

        // Assert
        Assert.False(options.Enabled);
        Assert.Equal(10, options.AbsoluteExpirationMinutes);
        Assert.Equal(3, options.SlidingExpirationMinutes);
    }

    [Fact]
    public void DefaultValues_AreCorrect()
    {
        // Arrange
        var options = new CachingOptions();

        // Assert
        Assert.True(options.Enabled);
        Assert.Equal(5, options.AbsoluteExpirationMinutes);
        Assert.Equal(2, options.SlidingExpirationMinutes);
    }
}
