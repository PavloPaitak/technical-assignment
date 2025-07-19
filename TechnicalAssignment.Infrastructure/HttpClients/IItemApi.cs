using Refit;
using TechnicalAssignment.Core;

namespace TechnicalAssignment.Infrastructure.HttpClients;

public interface IItemApi
{
    // todo: pp can be done with dto and mapped into some model but for simplicity I used the same contract and only one model across the project
    [Get("/mgtest/showcase.json?sp=r&st=2025-07-18T11:44:21Z&se=2025-07-28T19:59:21Z&spr=https&sv=2024-11-04&sr=b&sig=J8JFH7yvmkxaT87zRL03b%2F2wBa%2B987qyb%2BDBVBBkSo4%3D")]
    Task<List<Item>> GetItems();
}
