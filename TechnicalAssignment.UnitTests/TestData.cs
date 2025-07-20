using Bogus;
using TechnicalAssignment.Core;

namespace TechnicalAssignment.UnitTests;

public static class TestData
{
    public static Faker<CastMember> CastFaker { get; } = new Faker<CastMember>()
        .RuleFor(c => c.Name, f => f.Name.FullName());

    public static Faker<Director> DirectorFaker { get; } = new Faker<Director>()
        .RuleFor(d => d.Name, f => f.Name.FullName());

    public static Faker<Image> ImageFaker { get; } = new Faker<Image>()
        .RuleFor(i => i.Url, f => f.Image.PicsumUrl())
        .RuleFor(i => i.CachedUrl, f => f.Internet.Url())
        .RuleFor(i => i.H, f => f.Random.Int(100, 800))
        .RuleFor(i => i.W, f => f.Random.Int(100, 1200));

    public static Faker<VideoAlternative> VideoAlternativeFaker { get; } = new Faker<VideoAlternative>()
        .RuleFor(a => a.Quality, f => f.PickRandom("360p", "480p", "720p", "1080p"))
        .RuleFor(a => a.Url, f => f.Internet.Url());

    public static Faker<Video> VideoFaker { get; } = new Faker<Video>()
        .RuleFor(v => v.Title, f => f.Lorem.Sentence(3))
        .RuleFor(v => v.Type, f => f.PickRandom("Trailer", "Clip", "Featurette"))
        .RuleFor(v => v.Url, f => f.Internet.Url())
        .RuleFor(v => v.Alternatives, _ => VideoAlternativeFaker.Generate(2));

    public static Faker<ViewingWindow> ViewingWindowFaker { get; } = new Faker<ViewingWindow>()
        .RuleFor(v => v.StartDate, f => f.Date.Past(1).ToString("yyyy-MM-dd"))
        .RuleFor(v => v.EndDate, f => f.Date.Soon().ToString("yyyy-MM-dd"))
        .RuleFor(v => v.WayToWatch, f => f.PickRandom("Streaming", "On Demand"))
        .RuleFor(v => v.Title, f => $"{f.Company.CatchPhrase()} Special");

    public static Faker<Item> ItemFaker { get; } = new Faker<Item>()
        .RuleFor(i => i.Id, f => f.Random.Guid().ToString())
        .RuleFor(i => i.Headline, f => f.Lorem.Sentence())
        .RuleFor(i => i.Body, f => f.Lorem.Paragraph())
        .RuleFor(i => i.Cast, _ => CastFaker.Generate(3))
        .RuleFor(i => i.Directors, _ => DirectorFaker.Generate(1))
        .RuleFor(i => i.Genres, f => f.Lorem.Words(3).ToList())
        .RuleFor(i => i.Class, f => f.PickRandom("Movie", "Series"))
        .RuleFor(i => i.Cert, f => f.PickRandom("PG-13", "R", "G"))
        .RuleFor(i => i.Duration, f => f.Random.Int(80, 180))
        .RuleFor(i => i.Year, f => f.Date.Past(10).Year.ToString())
        .RuleFor(i => i.LastUpdated, f => f.Date.Recent().ToString("yyyy-MM-dd"))
        .RuleFor(i => i.Quote, f => f.Lorem.Sentence())
        .RuleFor(i => i.Rating, f => f.Random.Int(1, 10))
        .RuleFor(i => i.ReviewAuthor, f => f.Person.FullName)
        .RuleFor(i => i.Sum, f => f.Lorem.Sentence(5))
        .RuleFor(i => i.Synopsis, f => f.Lorem.Paragraph())
        .RuleFor(i => i.Url, f => f.Internet.Url())
        .RuleFor(i => i.SkyGoId, f => f.Random.AlphaNumeric(8))
        .RuleFor(i => i.SkyGoUrl, f => f.Internet.Url())
        .RuleFor(i => i.CardImages, _ => ImageFaker.Generate(2))
        .RuleFor(i => i.KeyArtImages, _ => ImageFaker.Generate(1))
        .RuleFor(i => i.Videos, _ => VideoFaker.Generate(2))
        .RuleFor(i => i.ViewingWindow, _ => ViewingWindowFaker.Generate());

    public static List<Item> Items(int count) => ItemFaker.Generate(count);

    public static Item Item() => ItemFaker.Generate();
}
