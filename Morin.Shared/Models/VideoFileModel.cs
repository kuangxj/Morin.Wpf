using System.Diagnostics.CodeAnalysis;

namespace Morin.Shared.Models;

public class VideoFileModel
{
    public Guid VideoId { get; set; }
    public int Episodes { get; set; }
    public string? VideoUrl { get; set; }
}
/// <summary>
/// 对比差异，针对多来源未做对比
/// </summary>
public class VideoFileModelComparer : EqualityComparer<VideoFileModel>
{
    public override bool Equals(VideoFileModel? x, VideoFileModel? y)
    {
        return x.VideoId == y.VideoId && x.Episodes == y.Episodes;
    }

    public override int GetHashCode([DisallowNull] VideoFileModel obj)
    {
        return obj.GetHashCode();
    }
}
