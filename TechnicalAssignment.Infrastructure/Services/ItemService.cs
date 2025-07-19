using TechnicalAssignment.Application.Services;
using TechnicalAssignment.Core;
using TechnicalAssignment.Infrastructure.HttpClients;

namespace TechnicalAssignment.Infrastructure.Services;

public class ItemService : IItemService
{
    private readonly IItemApi _api;

    public ItemService(IItemApi api)
    {
        _api = api;
    }

    public async Task<IReadOnlyList<Item>> GetItems()
    {
        var items = await _api.GetItems();
        return items;
    }

    public async Task<Item?> GetItem(string id)
    {
        var items = await GetItems();
        return items.FirstOrDefault(x => x.Id == id);
    }
}
