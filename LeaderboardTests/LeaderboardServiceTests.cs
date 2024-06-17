using Moq;
using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Models;
using LeaderboardBackEnd.Services;
using Serilog;

namespace LeaderboardTests;

public class LeaderboardServiceTests
{
    private readonly Mock<IDatabaseRepository> _databaseMock;
    private readonly Mock<IMongoDBRepository> _cacheMock;
    private readonly Mock<ICreationService> _creationServiceMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly LeaderboardService _service;

    public LeaderboardServiceTests()
    {
        _databaseMock = new Mock<IDatabaseRepository>();
        _cacheMock = new Mock<IMongoDBRepository>();
        _creationServiceMock = new Mock<ICreationService>();
        _loggerMock = new Mock<ILogger>();
        _service = new LeaderboardService(_databaseMock.Object, _cacheMock.Object, _creationServiceMock.Object);
    }

    [Fact]
    public void ToggleCacheCleaning_ShouldTurnOnCacheCleaning_WhenCacheCleaningIsOff()
    {
        // Arrange
        _service.GetType().GetProperty("CacheCleaningON").SetValue(_service, false);
        int cachePeriod = 10;

        // Act
        _service.ToggleCacheCleaning(cachePeriod);

        // Assert
        _cacheMock.Verify(cache => cache.TruncateDatabaseStart(cachePeriod * 1000), Times.Once);
        Assert.True((bool)_service.GetType().GetProperty("CacheCleaningON").GetValue(_service));
    }

    [Fact]
    public void ToggleCacheCleaning_ShouldTurnOffCacheCleaning_WhenCacheCleaningIsOn()
    {
        // Arrange
        _service.GetType().GetProperty("CacheCleaningON").SetValue(_service, true);

        // Act
        _service.ToggleCacheCleaning(It.IsAny<int>());

        // Assert
        _cacheMock.Verify(cache => cache.TruncateDatabaseStop(), Times.Once);
        Assert.False((bool)_service.GetType().GetProperty("CacheCleaningON").GetValue(_service));
    }

    [Fact]
    public async Task InsertRandomScoreAsync_ShouldLogError_WhenScoreIsNotInserted()
    {
        // Arrange
        var score = new Score();
        _creationServiceMock.Setup(cs => cs.CreateRandomScoreAsync()).Returns(score);
        _databaseMock.Setup(db => db.InsertScore(score))
                     .Returns(false)
                     .Verifiable();

        // Act
        await _service.InsertRandomScoreAsync();

        // Assert
        _databaseMock.Verify(db => db.InsertScore(It.IsAny<Score>()), Times.Once);
        _cacheMock.Verify(cache => cache.InsertScoreAsync(It.IsAny<Score>()), Times.Never);
    }

    [Fact]
    public async Task InsertLevelAsync_ShouldLogErrorAndReturnFalse_WhenInsertionFails()
    {
        // Arrange
        var level = new Level(1);
        _databaseMock.Setup(db => db.InsertLevel(level)).Returns(false).Verifiable();

        // Act
        var result = await _service.InsertLevelAsync(level);

        // Assert
        Assert.False(result);
        _databaseMock.Verify(db => db.InsertLevel(level), Times.Once);
        _cacheMock.Verify(cache => cache.InsertLevelAsync(It.IsAny<Level>()), Times.Never);
    }

    [Fact]
    public async Task GetLevelAsync_ShouldReturnLevelFromCache_WhenItExistsInCache()
    {
        // Arrange
        var level = new Level(1);
        _cacheMock.Setup(cache => cache.GetLevelAsync(It.IsAny<int>())).ReturnsAsync(level);

        // Act
        var result = await _service.GetLevelAsync(1);

        // Assert
        Assert.Equal(level, result);
        _cacheMock.Verify(cache => cache.GetLevelAsync(It.IsAny<int>()), Times.Once);
        _databaseMock.Verify(db => db.GetLevel(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetLevelAsync_ShouldReturnLevelFromDatabaseAndCacheIt_WhenItDoesNotExistInCache()
    {
        // Arrange
        var level = new Level(1);
        _cacheMock.Setup(cache => cache.GetLevelAsync(It.IsAny<int>())).ReturnsAsync((Level)null);
        _databaseMock.Setup(db => db.GetLevel(It.IsAny<int>())).Returns(level);

        // Act
        var result = await _service.GetLevelAsync(1);

        // Assert
        Assert.Equal(level, result);
        _cacheMock.Verify(cache => cache.GetLevelAsync(It.IsAny<int>()), Times.Once);
        _databaseMock.Verify(db => db.GetLevel(It.IsAny<int>()), Times.Once);
        _cacheMock.Verify(cache => cache.InsertLevelAsync(It.IsAny<Level>()), Times.Once);
    }

    [Fact]
    public async Task UpdateLevelAsync_ShouldLogErrorAndReturnFalse_WhenUpdateFails()
    {
        // Arrange
        var level = new Level(1);
        _databaseMock.Setup(db => db.UpdateLevel(level)).Returns(false).Verifiable();

        // Act
        var result = await _service.UpdateLevelAsync(level);

        // Assert
        Assert.False(result);
        _databaseMock.Verify(db => db.UpdateLevel(level), Times.Once);
        _cacheMock.Verify(cache => cache.UpdateLevelAsync(It.IsAny<Level>()), Times.Never);
    }

    [Fact]
    public async Task DeleteLevelAsync_ShouldReturnTrue_WhenDeletionIsSuccessful()
    {
        // Arrange
        _databaseMock.Setup(db => db.DeleteLevel(It.IsAny<int>())).Returns(true).Verifiable();

        // Act
        var result = await _service.DeleteLevelAsync(1);

        // Assert
        Assert.True(result);
        _databaseMock.Verify(db => db.DeleteLevel(It.IsAny<int>()), Times.Once);
        _cacheMock.Verify(cache => cache.DeleteLevelAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteLevelAsync_ShouldLogErrorAndReturnFalse_WhenDeletionFails()
    {
        // Arrange
        _databaseMock.Setup(db => db.DeleteLevel(It.IsAny<int>())).Returns(false).Verifiable();

        // Act
        var result = await _service.DeleteLevelAsync(1);

        // Assert
        Assert.False(result);
        _databaseMock.Verify(db => db.DeleteLevel(It.IsAny<int>()), Times.Once);
        _cacheMock.Verify(cache => cache.DeleteLevelAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task InsertPlayerAsync_ShouldLogErrorAndReturnFalse_WhenInsertionFails()
    {
        // Arrange
        var player = new Player();
        _databaseMock.Setup(db => db.InsertPlayer(player)).Returns(false).Verifiable();

        // Act
        var result = await _service.InsertPlayerAsync(player);

        // Assert
        Assert.False(result);
        _databaseMock.Verify(db => db.InsertPlayer(player), Times.Once);
        _cacheMock.Verify(cache => cache.InsertPlayerAsync(It.IsAny<Player>()), Times.Never);
    }

    [Fact]
    public async Task GetPlayerAsync_ShouldReturnPlayerFromCache_WhenItExistsInCache()
    {
        // Arrange
        var player = new Player();
        _cacheMock.Setup(cache => cache.GetPlayerAsync(It.IsAny<int>())).ReturnsAsync(player);

        // Act
        var result = await _service.GetPlayerAsync(1);

        // Assert
        Assert.Equal(player, result);
        _cacheMock.Verify(cache => cache.GetPlayerAsync(It.IsAny<int>()), Times.Once);
        _databaseMock.Verify(db => db.GetPlayer(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetPlayerAsync_ShouldReturnPlayerFromDatabaseAndCacheIt_WhenItDoesNotExistInCache()
    {
        // Arrange
        var player = new Player();
        _cacheMock.Setup(cache => cache.GetPlayerAsync(It.IsAny<int>())).ReturnsAsync((Player)null);
        _databaseMock.Setup(db => db.GetPlayer(It.IsAny<int>())).Returns(player);

        // Act
        var result = await _service.GetPlayerAsync(1);

        // Assert
        Assert.Equal(player, result);
        _cacheMock.Verify(cache => cache.GetPlayerAsync(It.IsAny<int>()), Times.Once);
        _databaseMock.Verify(db => db.GetPlayer(It.IsAny<int>()), Times.Once);
        _cacheMock.Verify(cache => cache.InsertPlayerAsync(It.IsAny<Player>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePlayerAsync_ShouldLogErrorAndReturnFalse_WhenUpdateFails()
    {
        // Arrange
        var player = new Player();
        _databaseMock.Setup(db => db.UpdatePlayer(player)).Returns(false).Verifiable();

        // Act
        var result = await _service.UpdatePlayerAsync(player);

        // Assert
        Assert.False(result);
        _databaseMock.Verify(db => db.UpdatePlayer(player), Times.Once);
        _cacheMock.Verify(cache => cache.UpdatePlayerAsync(It.IsAny<Player>()), Times.Never);
    }

    [Fact]
    public async Task DeletePlayerAsync_ShouldReturnTrue_WhenDeletionIsSuccessful()
    {
        // Arrange
        _databaseMock.Setup(db => db.DeletePlayer(It.IsAny<int>())).Returns(true).Verifiable();

        // Act
        var result = await _service.DeletePlayerAsync(1);

        // Assert
        Assert.True(result);
        _databaseMock.Verify(db => db.DeletePlayer(It.IsAny<int>()), Times.Once);
        _cacheMock.Verify(cache => cache.DeletePlayerAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeletePlayerAsync_ShouldLogErrorAndReturnFalse_WhenDeletionFails()
    {
        // Arrange
        _databaseMock.Setup(db => db.DeletePlayer(It.IsAny<int>())).Returns(false).Verifiable();

        // Act
        var result = await _service.DeletePlayerAsync(1);

        // Assert
        Assert.False(result);
        _databaseMock.Verify(db => db.DeletePlayer(It.IsAny<int>()), Times.Once);
        _cacheMock.Verify(cache => cache.DeletePlayerAsync(It.IsAny<int>()), Times.Never);
    }
}