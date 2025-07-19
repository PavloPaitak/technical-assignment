namespace TechnicalAssignment.Core;

public class Item
{
    public string Id { get; set; } = null!;
    public string Headline { get; set; } = null!;
    public string Body { get; set; } = null!;

    public List<CastMember> Cast { get; set; } = new();
    public List<Director> Directors { get; set; } = new();
    public List<string> Genres { get; set; } = new();

    public string Class { get; set; } = null!;
    public string Cert { get; set; } = null!;
    public int Duration { get; set; }
    public string Year { get; set; } = null!;
    public string LastUpdated { get; set; } = null!;

    public string? Quote { get; set; }
    public int? Rating { get; set; }
    public string? ReviewAuthor { get; set; }
    public string? Sum { get; set; }
    public string? Synopsis { get; set; }

    public string Url { get; set; } = null!;

    public string? SkyGoId { get; set; }
    public string? SkyGoUrl { get; set; }

    public List<Image> CardImages { get; set; } = new();
    public List<Image> KeyArtImages { get; set; } = new();

    public List<Video> Videos { get; set; } = new();
    public ViewingWindow? ViewingWindow { get; set; }
}

public class CastMember
{
    public string Name { get; set; } = null!;
}

public class Director
{
    public string Name { get; set; } = null!;
}

public class Image
{
    public string Url { get; set; } = null!;
    public string CachedUrl { get; set; } = null!;
    public int H { get; set; }
    public int W { get; set; }
}

public class Video
{
    public string Title { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Url { get; set; } = null!;
    public List<VideoAlternative> Alternatives { get; set; } = new();
}

public class VideoAlternative
{
    public string Quality { get; set; } = null!;
    public string Url { get; set; } = null!;
}

public class ViewingWindow
{
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? WayToWatch { get; set; }
    public string? Title { get; set; }
}
