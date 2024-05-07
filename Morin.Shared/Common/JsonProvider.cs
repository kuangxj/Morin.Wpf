using Newtonsoft.Json;
namespace Morin.Shared.Common;

public class JsonProvider
{
    public static T? FromContentToObject<T>(string jsonContent)
    {
        return JsonConvert.DeserializeObject<T>(jsonContent);
    }
    public static T? ToObject<T>(string jsonPath)
    {
        if (!string.IsNullOrEmpty(jsonPath))
        {
            if (File.Exists(jsonPath))
            {
                var jsonContent = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<T>(jsonContent);
            }
        }
        return default;
    }
    public static T? FromPathToObject<T>(string? jsonPath)
    {
        if (!string.IsNullOrEmpty(jsonPath))
        {
            if (File.Exists(jsonPath))
            {
                var jsonContent = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<T>(jsonContent);
            }
        }
        return default;
    }
    public static List<T>? FromPathToObjects<T>(string? jsonPath)
    {
        if (!string.IsNullOrEmpty(jsonPath))
        {
            if (File.Exists(jsonPath))
            {
                var jsonContent = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<List<T>>(jsonContent);
            }
        }
        return null;
    }
    public static void FromContentToFile<T>(string? path, IEnumerable<T> list)
    {
        if (!string.IsNullOrEmpty(path) && list != null)
        {
            var directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(list));
        }
    }
    public static void FromContentToFile<T>(string? path, T t)
    {
        if (!string.IsNullOrEmpty(path) && t != null)
        {
            var directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(t));
        }
    }
}
