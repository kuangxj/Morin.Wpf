using Newtonsoft.Json;

namespace Morin.Shared.Models;

public class RspJsonClassModel
{
    [JsonProperty("type_id")]
    public int Id { get; set; }

    [JsonProperty("type_pid")]
    public int Pid { get; set; }

    [JsonProperty("type_name")]
    public string? ClassName { get; set; }

    public int SourceID { get; set; }

    public List<RspJsonClassModel> ChildNodes { get; set; }
    public RspJsonClassModel()
    {
        ChildNodes = [];
        Pid = 0;
    }
}
