namespace TechnicalAssignment.Application.Services;

public interface IImageCacheService
{
    Task<string> GetCachedImageUrl(string externalUrl);
}
