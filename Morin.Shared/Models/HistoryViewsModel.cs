using Newtonsoft.Json;

namespace Morin.Shared.Models;

public class HistoryViewsModel : Model
{
    [JsonIgnore]
    public string Key => $"{VodSourceID}|{VodId}|{Episode}";
    public int VodSourceID { get; set; }
    [JsonIgnore]
    public string? VodSourceTitle { get; set; }
    public int VodId { get; set; }
    public string? Episode { get; set; }
    public string? VodPlayUrl { get; set; }
    public string? VodPic { get; set; }
    public string? VodName { get; set; }
    public string? VodRemarks { get; set; }
    /// <summary>
    /// 观看时间
    /// </summary>
    public DateTime ViewTime { get; set; } = DateTime.Now;
}
