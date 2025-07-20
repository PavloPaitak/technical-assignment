using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TechnicalAssignment.Application.Services;
using TechnicalAssignment.Core;
using TechnicalAssignment.Infrastructure.HttpClients;

namespace TechnicalAssignment.Infrastructure.Services;

public class ItemService : IItemService
{
    private const string AllItemsKey = "AllItems";

    private readonly IItemApi _api;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _ttl;

    public ItemService(
        IItemApi api,
        IMemoryCache cache,
        IOptions<ItemServiceSettings> options)
    {
        _api = api;
        _cache = cache;
        _ttl = options.Value.Ttl;
    }

    public async Task<IReadOnlyList<Item>> GetItems()
    {
        if (_cache.TryGetValue(AllItemsKey, out IReadOnlyList<Item>? items)) return items ?? [];

        items = await _api.GetItems();
        _cache.Set(AllItemsKey, items, _ttl);

        foreach (var item in items)
        {
            var key = GetItemKey(item.Id);
            _cache.Set(key, item, _ttl);
        }

        return items;
    }

    public async Task<Item?> GetItem(string id)
    {
        var key = GetItemKey(id);
        if (_cache.TryGetValue(key, out Item? cached)) return cached;

        var items = await GetItems();

        return items.FirstOrDefault(x => x.Id == id);
    }

    private static string GetItemKey(string id)
    {
        return $"Item:{id}";
    }
}

public class ItemServiceSettings
{
    public const string SectionName = "ItemService";

    public TimeSpan Ttl { get; set; }
}
