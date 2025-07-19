using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TechnicalAssignment.Application.Services;

namespace TechnicalAssignment.Infrastructure.Services;

public class ImageCacheService : IImageCacheService
{
    private const string FallbackImageUrl = "/images/placeholder.webp"; // todo: pp can be moved to appsettings

    private readonly IMemoryCache _cache;
    private readonly HttpClient _http;
    private readonly ILogger<ImageCacheService> _logger;
    private readonly TimeSpan _ttl = TimeSpan.FromDays(1); // todo: pp can be moved to appsettings

    public ImageCacheService(
        IMemoryCache cache,
        HttpClient http,
        ILogger<ImageCacheService> logger)
    {
        _cache = cache;
        _http = http;
        _logger = logger;
    }

    public async Task<string> GetCachedImageUrl(string externalUrl)
    {
        var key = ComputeKey(externalUrl);
        if (_cache.TryGetValue<ImageCacheEntry>(key, out var entry)) return BuildCachedImageUrl(key);

        var (bytes, contentType) = await FetchImage(externalUrl);
        if (bytes == null || contentType == null) return FallbackImageUrl;

        entry = new ImageCacheEntry(bytes, contentType);
        _cache.Set(key, entry, _ttl);

        return BuildCachedImageUrl(key);
    }

    private async Task<(byte[]? Bytes, string? ContentType)> FetchImage(string externalUrl)
    {
        try
        {
            return await TryFetchImage(externalUrl);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Network error fetching image from {Url}: {Message}", externalUrl, ex.Message);
            return (null, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching image from {Url}: {Message}", externalUrl, ex.Message);
            return (null, null);
        }
    }

    private async Task<(byte[]? Bytes, string? ContentType)> TryFetchImage(string externalUrl)
    {
        var response = await _http.GetAsync(externalUrl);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Image at {Url} returned HTTP {StatusCode}", externalUrl, (int)response.StatusCode);
            return (null, null);
        }

        var mediaType = response.Content.Headers.ContentType?.MediaType;
        if (mediaType == null || !mediaType.StartsWith("image/"))
        {
            _logger.LogWarning("Non-image content from {Url}: {ContentType}", externalUrl, mediaType);
            return (null, null);
        }

        var bytes = await response.Content.ReadAsByteArrayAsync();
        return (bytes, mediaType);
    }

    private static string ComputeKey(string raw) // todo: pp can be encapsulated into separate class with interface
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(hash);
    }

    private static string BuildCachedImageUrl(string key) => $"/images/{key}"; // todo: pp can be moved to appsettings or encapsulated into separate class with interface

    public record ImageCacheEntry(byte[] Bytes, string ContentType); // todo: pp can be moved somewhere else
}
