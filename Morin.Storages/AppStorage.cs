using Morin.Shared.Common;
using Morin.Shared.Configs;
using Morin.Shared.Models;
using System.Collections.Concurrent;

namespace Morin.Storages;

public class AppStorage(AppSettingsConfig appSettingsConfig) : IAppStorage, IDisposable
{
    private readonly AppSettingsConfig appSettingsConfig = appSettingsConfig;

    public IEnumerable<MenuModel> Menus { get; set; } = [];
    public ConcurrentDictionary<int, IEnumerable<ClassModel>> ClasseDict { get; } = [];
    public ConcurrentDictionary<int, MediaSourceModel> MediaSourceDict { get; } = [];
    public ConcurrentDictionary<string, HistoryViewsModel> HistoryViewDict { get; set; } = [];
    public ConcurrentDictionary<string, PlaySkipTimeModel> PlaySkipTimeDict { get; set; } = [];
    public ConcurrentDictionary<string, HistorySearchModel> HistorySearchDict { get; set; } = [];
    public List<TVSourceModel> TVSources { get; set; } = [];
    public ConcurrentDictionary<string, FavoriteModel> FavoriteDict { get; set; } = [];

    public void Dispose()
    {
        //  持久化操作
        JsonProvider.FromContentToFile(appSettingsConfig.VideoSource, MediaSourceDict.Values);
        JsonProvider.FromContentToFile(appSettingsConfig.TVSource, TVSources);
        JsonProvider.FromContentToFile(appSettingsConfig.HistoryViews, HistoryViewDict.Values);
        JsonProvider.FromContentToFile(appSettingsConfig.PlaySkipTimes, PlaySkipTimeDict.Values);
        JsonProvider.FromContentToFile(appSettingsConfig.HistorySearchs, HistorySearchDict.Values);
        JsonProvider.FromContentToFile(appSettingsConfig.Favorites, FavoriteDict.Values);

        GC.SuppressFinalize(this);
    }
}
