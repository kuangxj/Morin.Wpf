using System.Xml.Serialization;

namespace Morin.Shared.Models;

public class RspXmlDataModel
{
    [XmlAttribute(AttributeName = "page")]
    public string? PageIndex { get; set; }

    [XmlAttribute(AttributeName = "pagecount")]
    public string? PageCount { get; set; }

    [XmlAttribute(AttributeName = "pagesize")]
    public string? PageSize { get; set; }

    [XmlAttribute(AttributeName = "recordcount")]
    public string? Total { get; set; }

    [XmlElement("video")]
    public RspXmlVideoModel[] Videos { get; set; }
}
public class RspXmlVideoModel
{
    [XmlElement("last")]
    public string? VodTime { get; set; }
    [XmlElement("id")]
    public string? VodId { get; set; }
    [XmlElement("tid")]
    public string? Pid { get; set; }
    [XmlElement("name")]
    public string? VodName { get; set; }
    [XmlElement("type")]
    public string? TypeName { get; set; }
    [XmlElement("dt")]
    public string? VodPlayNote { get; set; }
    [XmlElement("note")]
    public string? VodDownNote { get; set; }
    [XmlElement("area")]
    public string? VodArea { get; set; }
    [XmlElement("lang")]
    public string? VodLang { get; set; }

    [XmlElement("pic")]
    public string? VodPic { get; set; }
    [XmlElement("actor")]
    public string? VodActor { get; set; }
    [XmlElement("director")]
    public string? VodDirector { get; set; }
    [XmlElement("year")]
    public string? VodYear { get; set; }
    [XmlElement("state")]
    public string? VodState { get; set; }
    [XmlElement("dl")]
    public RspXmlVideoDLModel DL { get; set; }
    [XmlElement("des")]
    public string? VodRemarks { get; set; }

}
public class RspXmlVideoDLModel
{
    [XmlElement("dd")]
    public List<RspXmlVideoDDModel> RspXmlVideoDDList { get; set; }
}

public class RspXmlTyModel
{
    [XmlAttribute(AttributeName = "id")]
    public int id { get; set; }
    [XmlText]
    public string? Text { get; set; }
}


public class RspXmlClassModel
{
    [XmlElement("ty")]
    public RspXmlTyModel[] Ty { get; set; }
}
[XmlRoot("rss")]
public class RspVideoListXmlRootModel
{
    [XmlAttribute(AttributeName = "version")]
    public string? Version { get; set; }

    [XmlElement("list")]
    public RspXmlDataModel Videos { get; set; }

    [XmlElement("class")]
    public RspXmlClassModel Classes { get; set; }

}


public class RspXmlVideoDDModel
{
    [XmlAttribute(AttributeName = "flag")]
    public string? Flag { get; set; }
    [XmlText]
    public string? Text { get; set; }

}
