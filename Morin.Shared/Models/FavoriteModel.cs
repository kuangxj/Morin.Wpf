﻿using Newtonsoft.Json;

namespace Morin.Shared.Models;

public class FavoriteModel
{
    [JsonIgnore]
    public string Key => $"{SourceID}|{VodId}|{Episode}";
    public int SourceID { get; set; }
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