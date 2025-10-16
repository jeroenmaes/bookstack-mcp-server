using BookStackMcpServer.Models;

namespace BookStackMcpServer.Tests.Models;

public class BookStackOptionsTests
{
    [Fact]
    public void SectionName_ReturnsCorrectValue()
    {
        // Assert
        Assert.Equal("BookStack", BookStackOptions.SectionName);
    }

    [Fact]
    public void Properties_CanBeSet()
    {
        // Arrange
        var options = new BookStackOptions
        {
            BaseUrl = "https://example.com",
            EnableWrite = true
        };

        // Assert
        Assert.Equal("https://example.com", options.BaseUrl);
        Assert.True(options.EnableWrite);
    }

    [Fact]
    public void DefaultValues_AreCorrect()
    {
        // Arrange
        var options = new BookStackOptions();

        // Assert
        Assert.Equal(string.Empty, options.BaseUrl);
        Assert.False(options.EnableWrite);
    }

    [Fact]
    public void EnableWrite_DefaultsToFalse()
    {
        // Arrange
        var options = new BookStackOptions();

        // Assert
        Assert.False(options.EnableWrite);
    }

    [Fact]
    public void EnableWrite_CanBeSetToTrue()
    {
        // Arrange
        var options = new BookStackOptions
        {
            BaseUrl = "https://example.com",
            EnableWrite = true
        };

        // Assert
        Assert.True(options.EnableWrite);
    }
}
