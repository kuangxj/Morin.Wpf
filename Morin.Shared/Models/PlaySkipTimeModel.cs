using Newtonsoft.Json;

namespace Morin.Shared.Models;

public class PlaySkipTimeModel
{
    [JsonIgnore]
    public string Key => $"{SourceID}|{VodId}";
    public int SourceID { get; set; }
    public int VodId { get; set; }
    /// <summary>
    /// 跳过开头时间
    /// </summary>
    public int SkipBeginPosition { get; set; }
    /// <summary>
    /// 跳过结尾时间
    /// </summary>
    public int SkipEndPosition { get; set; }
    public bool SkipSwitch { get; set; }
}
