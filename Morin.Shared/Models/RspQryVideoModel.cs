namespace Morin.Shared.Models;

public class RspQryVideoModel
{
    public string? Msg { get; set; }
    public int PageIndex { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }

    public List<VideoModel> Videos { get; set; }
    public List<ClassModel> Classes { get; set; }
}
