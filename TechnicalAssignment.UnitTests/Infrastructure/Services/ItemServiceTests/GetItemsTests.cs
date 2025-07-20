namespace TechnicalAssignment.UnitTests.Infrastructure.Services.ItemServiceTests;

public class GetItemsTests : BaseItemServiceTests
{
    [Fact]
    public async Task CacheMiss_CallsApiAndSeedsCache()
    {
        // Arrange
        _api.Setup(x => x.GetItems()).ReturnsAsync(_testItems);

        // Act
        var items = await _itemService.GetItems();

        // Assert
        _api.Verify(x => x.GetItems(), Times.Once);
        Assert.Equal(_testItems, items);

        Assert.True(_cache.TryGetValue("AllItems", out _));
        foreach (var testItem in _testItems) Assert.True(_cache.TryGetValue($"Item:{testItem.Id}", out _));
    }

    [Fact]
    public async Task CacheHit_DoesNotCallApi()
    {
        // Arrange
        _cache.Set("AllItems", _testItems, TimeSpan.FromMinutes(5));

        // Act
        var items = await _itemService.GetItems();

        // Assert
        _api.Verify(x => x.GetItems(), Times.Never);
        Assert.Equal(_testItems, items);
    }
}
