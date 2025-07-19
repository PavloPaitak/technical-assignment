using TechnicalAssignment.Core;

namespace TechnicalAssignment.Application.Services;

public interface IItemService
{
    Task<IReadOnlyList<Item>> GetItems();

    Task<Item?> GetItem(string id);
}
