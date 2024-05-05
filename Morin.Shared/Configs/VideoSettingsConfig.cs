using Morin.Shared.Common;

namespace Morin.Shared.Configs;

public class VideoSettingsConfig
{
    public int Volume { get; set; }
    public bool VideosSortByAsc { get; set; }

    public void Save(string path)
    {
        JsonProvider.FromContentToFile(path, this);
    }

}
