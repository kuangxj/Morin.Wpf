namespace Morin.Shared.Models;

public class MediaSourceModel : Model
{
    public MediaSourceModel(int id)
    {
        Id = id;
    }
    public string? Title { get; set; }
    public string? JsonUri { get; set; }
    public string? XmlUri { get; set; }
    public string? ParsingUri { get; set; }

    public int Total { get; set; }
}
