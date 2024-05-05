using System.ComponentModel;

namespace Morin.Shared.Models;
public enum PlayMode
{
    [Description("视频 ")] Video,
    [Description("电视")] TV,
    [Description("直播")] LIVE,

}
public enum NetworkCarrierType
{
    [Description("其他通信 ")] Other,
    [Description("中国移动")] CMCC,
    [Description("中国联通")] CUCC,
    [Description("中国电信 ")] CTCC, 

}

public enum ApiCmsType
{
    [Description("App")] AppCms,
    [Description("Sea")] SeaCms,
}
/// <summary>
/// Api 协议类型
/// </summary>
public enum ApiProtocolType
{
    [Description("Json")] Json,
    [Description("Xml")] Xml,
}
/// <summary>
/// 过滤的类型
/// </summary>
public enum FilterCategory
{
    [Description("电影")] Movie,
    [Description("动漫")] Anime,
    [Description("剧集")] TV,
    [Description("记录")] Record,
    [Description("综艺")] Entertainment,
    [Description("None")] None
}

/// <summary>
/// 下载状态
/// </summary>
public enum DownloadState
{
    [Description("播放")] Downloading,
    [Description("停止")] DownloadStop,
    [Description("暂停")] DownloadPause,
    [Description("完成")] DownloadComplete
}

/// <summary>
/// 下载的协议类型
/// </summary>
public enum DownloadProtoType
{
    [Description("标准")] Standard,
    [Description("磁力链")] Magnet,
    [Description("VLC播放器")] VLC,
    [Description("其他")] Other,
}
