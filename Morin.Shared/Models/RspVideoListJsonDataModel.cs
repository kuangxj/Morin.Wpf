using Newtonsoft.Json;

namespace Morin.Shared.Models;

public class RspVideoListJsonDataModel
{
    [JsonProperty("code")]
    public int Code { get; set; }
    [JsonProperty("msg")]
    public string? Msg { get; set; }

    [JsonProperty("page")]
    public int PageIndex { get; set; }

    [JsonProperty("pagecount")]
    public int PageCount { get; set; }

    [JsonProperty("limit")]
    public int PageSize { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("list")]
    public List<RspJsonVideoModel> Videos { get; set; }
    [JsonProperty("class")]
    public List<RspJsonClassModel> Classes { get; set; }

}

