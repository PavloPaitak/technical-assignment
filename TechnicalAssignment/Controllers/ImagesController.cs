using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TechnicalAssignment.Infrastructure.Services;

namespace TechnicalAssignment.Controllers;

[ApiController]
[Route("images")]
public class ImagesController : ControllerBase
{
    private readonly IMemoryCache _cache;

    public ImagesController(IMemoryCache cache)
    {
        _cache = cache;
    }

    [HttpGet("{key}")]
    public IActionResult Get(string key)
    {
        if (!_cache.TryGetValue<ImageCacheService.ImageCacheEntry>(key, out var entry) || entry == null) return NotFound();
        return File(entry.Bytes, entry.ContentType);
    }
}

