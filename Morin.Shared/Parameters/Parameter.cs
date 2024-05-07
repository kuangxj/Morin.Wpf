namespace Morin.Shared.Parameters;

public class Parameter
{
    public int SourceID { get; set; }
    /// <summary>
    /// 参数名称
    /// </summary>
    public string? AcName { get; set; } = "list";
    /// <summary>
    /// 关键字
    /// </summary>
    public string? KeyWord { get; set; }
    /// <summary>
    /// 第几页
    /// </summary>
    public int PageIndex { get; set; }
    /// <summary>
    /// 默认20条/页
    /// </summary>
    public int PageSize { get; set; } = 20;
    /// <summary>
    /// 几小时之前的数据
    /// </summary>
    public int Hour { get; set; }
    /// <summary>
    /// 剧集ID，多个用英文逗号隔开
    /// </summary>
    public string? VodIds { get; set; }
    /// <summary>
    /// 分类ID
    /// </summary>
    public int ClassID { get; set; }
}
