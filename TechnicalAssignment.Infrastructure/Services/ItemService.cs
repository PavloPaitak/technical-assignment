using Microsoft.Extensions.Caching.Memory;
using TechnicalAssignment.Application.Services;
using TechnicalAssignment.Core;
using TechnicalAssignment.Infrastructure.HttpClients;

namespace TechnicalAssignment.Infrastructure.Services;

public class ItemService : IItemService
{
    private const string AllItemsKey = "AllItems";
    private readonly TimeSpan _itemsTtl = TimeSpan.FromMinutes(5); // todo: pp can be moved to appsettings

    private readonly IItemApi _api;
    private readonly IMemoryCache _cache;

    public ItemService(IItemApi api, IMemoryCache cache)
    {
        _api = api;
        _cache = cache;
    }

    public async Task<IReadOnlyList<Item>> GetItems()
    {
        if (_cache.TryGetValue(AllItemsKey, out IReadOnlyList<Item>? items)) return items ?? [];

        // Fetch fresh
        items = await _api.GetItems();

        // Cache all
        _cache.Set(AllItemsKey, items, _itemsTtl);

        // Cache per-id
        foreach (var item in items)
        {
            var key = GetItemKey(item.Id);
            _cache.Set(key, item, _itemsTtl);
        }

        return items;
    }

    public async Task<Item?> GetItem(string id)
    {
        var key = GetItemKey(id);

        // Get from cache
        if (_cache.TryGetValue(key, out Item? cached)) return cached;

        // Fallback: refetch all items and cache
        var items = await GetItems();
        var item = items.FirstOrDefault(x => x.Id == id);

        return item;
    }

    private static string GetItemKey(string id)
    {
        return $"Item:{id}";
    }
}
