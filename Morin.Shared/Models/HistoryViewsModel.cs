using Newtonsoft.Json;

namespace Morin.Shared.Models;

public class HistoryViewsModel : Model
{
    [JsonIgnore]
    public string Key => $"{SourceID}|{VodId}|{Episode}";
    public int SourceID { get; set; }
    public int VodId { get; set; }
    public string? Episode { get; set; }
    /// <summary>
    /// 观看时间
    /// </summary>
    public DateTime ViewTime { get; set; } = DateTime.Now;
}
