namespace TechnicalAssignment.UnitTests.Infrastructure.Services.ItemServiceTests;

public class GetItemTests : BaseItemServiceTests
{
    [Fact]
    public async Task CacheMiss_CallsApiAndSeedsCache()
    {
        // Arrange
        var testItem = _testItems[0];
        _api.Setup(x => x.GetItems()).ReturnsAsync(_testItems);

        // Act
        var item = await _itemService.GetItem(testItem.Id);

        // Assert
        _api.Verify(x => x.GetItems(), Times.Once);
        Assert.Equal(testItem, item);

        Assert.True(_cache.TryGetValue("AllItems", out _));
        Assert.True(_cache.TryGetValue($"Item:{testItem.Id}", out _));
    }

    [Fact]
    public async Task CacheHit_DoesNotCallApi()
    {
        // Arrange
        var testItem = _testItems[0];
        _cache.Set($"Item:{testItem.Id}", testItem, TimeSpan.FromMinutes(5));

        // Act
        var items = await _itemService.GetItem(testItem.Id);

        // Assert
        _api.Verify(x => x.GetItems(), Times.Never);
        Assert.Equal(testItem, items);
    }
}
