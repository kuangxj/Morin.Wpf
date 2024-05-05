

namespace Morin.Shared.Models;

public class TVSourceModel : IEquatable<TVSourceModel?>
{
    public string? GroupTitle { get; set; }
    public int Sort { get; set; }
    public IEnumerable<TVSourceDetailModel>? TVSourceDetails { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TVSourceModel);
    }

    public bool Equals(TVSourceModel? other)
    {
        return other is not null &&
               GroupTitle == other.GroupTitle;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GroupTitle);
    }

    public static bool operator ==(TVSourceModel? left, TVSourceModel? right)
    {
        return EqualityComparer<TVSourceModel>.Default.Equals(left, right);
    }

    public static bool operator !=(TVSourceModel? left, TVSourceModel? right)
    {
        return !(left == right);
    }
}
public class TVSourceDetailModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public NetworkCarrierType NetworkCarrierType { get; set; }
    public string? WebAddr { get; set; }
}
