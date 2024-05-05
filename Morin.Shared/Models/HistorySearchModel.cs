namespace Morin.Shared.Models;

public class HistorySearchModel
{
    public int Count { get; set; }
    public string? KeyWord { get; set; }
    public DateTime SearchTime { get; set; } = DateTime.Now;
}
