using Microsoft.Extensions.Options;

namespace TechnicalAssignment.UnitTests.Infrastructure.Services.ItemServiceTests;

public abstract class BaseItemServiceTests
{
    protected readonly Mock<IItemApi> _api = new();
    protected readonly MemoryCache _cache = new(new MemoryCacheOptions());
    protected readonly ItemService _itemService;
    protected readonly List<Item> _testItems = TestData.Items(3);
    
    protected BaseItemServiceTests()
    {
        _itemService = new ItemService(
            api: _api.Object,
            cache: _cache,
            options: Options.Create(new ItemServiceSettings { Ttl = TimeSpan.FromMinutes(5) }));
    }
}
