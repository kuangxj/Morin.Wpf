using System.Xml.Serialization;

namespace Morin.Shared.Common;

public class XmlProvider
{
    public static string? XmlSerialize<T>(T obj)
    {
        if (obj != null)
        {
            using var sw = new StringWriter();
            var serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(sw, obj);
            sw.Close();
            return sw.ToString();
        }
        return null;
    }

    public static T? DESerializer<T>(string? strXML) where T : class
    {
        using var sr = new StringReader(strXML);
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        return serializer.Deserialize(sr) as T;
    }
}
