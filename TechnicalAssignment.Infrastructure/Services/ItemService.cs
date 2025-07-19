using Microsoft.Extensions.Caching.Memory;
using TechnicalAssignment.Application.Services;
using TechnicalAssignment.Core;
using TechnicalAssignment.Infrastructure.HttpClients;

namespace TechnicalAssignment.Infrastructure.Services;

public class ItemService : IItemService
{
    private const string AllItemsKey = "AllItems";

    private readonly IItemApi _api;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _ttl = TimeSpan.FromMinutes(5); // todo: pp can be moved to appsettings

    public ItemService(IItemApi api, IMemoryCache cache)
    {
        _api = api;
        _cache = cache;
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
