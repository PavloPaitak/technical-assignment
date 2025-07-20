namespace TechnicalAssignment.Application.Services;

public interface IImageCacheService
{
    Task<string> CacheImage(string externalUrl);
}
