using Morin.Shared.Common;
using Morin.Shared.Configs;
using Morin.Shared.Models;
using Morin.Storages;

namespace Morin.Services;

public class AppService(IAppStorage appStorage, AppSettingsConfig appSettingsConfig) : IAppService
{
    private readonly IAppStorage appStorage = appStorage;
    private readonly AppSettingsConfig appSettingsConfig = appSettingsConfig;

    public void ClassesAddOrUpdate(int id, IEnumerable<ClassModel> classes)
    {
        appStorage.ClasseDict.AddOrUpdate(id, classes, (k, v) => classes);
    }

    public IEnumerable<ClassModel> GetClasses(int id)
    {
        if (appStorage.ClasseDict.TryGetValue(id, out var classes))
        {
            return classes;
        }
        return [];
    }

    public HistoryViewsModel GetHistoryView(string key)
    {
        if (appStorage.HistoryViewDict.TryGetValue(key, out var model))
        {
            return model;
        }
        return new();
    }

    public IEnumerable<HistoryViewsModel> GetHistoryViews(int sourceID, int vodId)
        => appStorage.HistoryViewDict.Values.Where(x => x.SourceID == sourceID && x.VodId == vodId);


    public IEnumerable<HistoryViewsModel> GetHistoryViews()
    {
        var historyViews = appStorage.HistoryViewDict.Values;
        if (historyViews.Count != 0)
        {
            return historyViews;
        }
        historyViews = JsonProvider.FromPathToObjects<HistoryViewsModel>(appSettingsConfig.HistoryViews);
        if (historyViews != null && historyViews.Count != 0)
        {
            foreach (var item in historyViews)
            {
                HistoryViewsAddOrUpdate(item);
            }

            return historyViews;
        }
        return [];
    }

    public IEnumerable<HistoryViewsModel> GetHistoryViews(int sourceID)
        => appStorage.HistoryViewDict.Values.Where(x => x.SourceID == sourceID);


    public IEnumerable<MediaSourceModel> GetMediaSources()
    {
        var mediaSources = appStorage.MediaSourceDict.Values;
        if (mediaSources.Count != 0)
        {
            return mediaSources;
        }
        mediaSources = JsonProvider.FromPathToObjects<MediaSourceModel>(appSettingsConfig.VideoSource);
        if (mediaSources != null && mediaSources.Count != 0)
        {
            foreach (var item in mediaSources)
            {
                MediaSourcesAddOrUpdate(item);
            }

            return mediaSources;
        }
        return [];
    }

    public IEnumerable<MenuModel> GetMenus()
    {
        return appStorage.Menus;
    }

    public PlaySkipTimeModel GetPlaySkipTime(string key)
    {
        if (appStorage.PlaySkipTimeDict.TryGetValue(key, out var playSkipTime))
        {
            return playSkipTime;
        }
        return new();
    }

    public IEnumerable<PlaySkipTimeModel> GetPlaySkipTimes()
    {
        var playSkipTimes = appStorage.PlaySkipTimeDict.Values;
        if (playSkipTimes.Count != 0)
        {
            return playSkipTimes;
        }
        playSkipTimes = JsonProvider.FromPathToObjects<PlaySkipTimeModel>(appSettingsConfig.PlaySkipTimes);
        if (playSkipTimes != null && playSkipTimes.Count != 0)
        {
            foreach (var item in playSkipTimes)
            {
                PlaySkipTimeAddOrUpdate(item);
            }

            return playSkipTimes;
        }
        return [];
    }

    public IEnumerable<PlaySkipTimeModel> GetPlaySkipTimes(int sourceID, int vodId)
       => appStorage.PlaySkipTimeDict.Values.Where(x => x.SourceID == sourceID && x.VodId == vodId);


    public void HistoryViewsAddOrUpdate(HistoryViewsModel historyView)
    {
        appStorage.HistoryViewDict.AddOrUpdate(historyView.Key, historyView, (k, v) => historyView);
    }

    public void HistoryViewsTryRemove(string key)
    {
        appStorage.HistoryViewDict.TryRemove(key, out _);
    }

    public void LoadHistoryViews()
    {
        var models = JsonProvider.FromPathToObjects<HistoryViewsModel>(appSettingsConfig.HistoryViews);
        if (models != null && models.Count != 0)
        {
            foreach (var item in models)
            {
                HistoryViewsAddOrUpdate(item);
            }
        }
    }

    public void LoadMediaSources()
    {
        var models = JsonProvider.FromPathToObjects<MediaSourceModel>(appSettingsConfig.VideoSource);
        if (models != null && models.Count != 0)
        {
            foreach (var item in models)
            {
                MediaSourcesAddOrUpdate(item);
            }
        }
    }

    public void LoadPlaySkipTimes()
    {
        var models = JsonProvider.FromPathToObjects<PlaySkipTimeModel>(appSettingsConfig.PlaySkipTimes);
        if (models != null && models.Count != 0)
        {
            foreach (var item in models)
            {
                PlaySkipTimeAddOrUpdate(item);
            }
        }
    }

    public VideoSettingsConfig GetVideoSettings()
    {
        return JsonProvider.FromPathToObject<VideoSettingsConfig>(appSettingsConfig.VideoConfig) ?? new();
    }

    public void MediaSourcesAddOrUpdate(MediaSourceModel media)
    {
        appStorage.MediaSourceDict.AddOrUpdate(media.Id, media, (k, v) => media);
    }

    public void MediaSourcesTryRemove(int id)
    {
        appStorage.MediaSourceDict.TryRemove(id, out _);
    }


    public void MenusSave(IEnumerable<MenuModel> menus)
    {
        appStorage.Menus = menus;
    }

    public void PlaySkipTimeAddOrUpdate(PlaySkipTimeModel skip)
    {
        appStorage.PlaySkipTimeDict.AddOrUpdate(skip.Key, skip, (k, v) => skip);
    }

    public void PlaySkipTimesTryRmove(string key)
    {
        appStorage.PlaySkipTimeDict.TryRemove(key, out _);
    }

    public PlaySkipTimeModel GetPlaySkipTime(int sourceID, int vodId)
    {
        var key = $"{sourceID}|{vodId}";
        return GetPlaySkipTime(key);
    }

    public IEnumerable<HistorySearchModel> GetHistorySearchs()
    {
        var models = appStorage.HistorySearchDict.Values;
        if (models.Count != 0)
        {
            return models;
        }
        models = JsonProvider.FromPathToObjects<HistorySearchModel>(appSettingsConfig.HistorySearchs);
        if (models != null && models.Count != 0)
        {
            foreach (var item in models)
            {
                HistorySearchAddOrUpdate(item);
            }

            return models;
        }
        return [];
    }

    public void HistorySearchAddOrUpdate(HistorySearchModel model)
    {
        appStorage.HistorySearchDict.AddOrUpdate(model.KeyWord, model, (k, v) => model);
    }

    public void HistorySearchTryRemove(string key)
    {
        appStorage.HistorySearchDict.TryRemove(key, out _);
    }

    public void LoadHistorySearchs()
    {
        var models = JsonProvider.FromPathToObjects<HistorySearchModel>(appSettingsConfig.HistorySearchs);
        if (models != null && models.Count != 0)
        {
            foreach (var item in models)
            {
                HistorySearchAddOrUpdate(item);
            }
        }
    }

    public List<TVSourceModel> GetTVSources()
    {
        var models = appStorage.TVSources;
        if (models.Count != 0)
        {
            return models;
        }
        models = JsonProvider.FromPathToObjects<TVSourceModel>(appSettingsConfig.TVSource);
        if (models != null && models.Count != 0)
        {
            TVSourceAddOrUpdate(models);  
            return models;
        }
        return [];
    }

    public void TVSourceAddOrUpdate(TVSourceModel model)
    {
        var index = appStorage.TVSources.IndexOf(model);
        if (index != -1)
        {
            appStorage.TVSources[index] = model;
        }
        appStorage.TVSources.Add(model);
    }

    public void TVSourceTryRemove(string groupTitle)
    {
        var model = appStorage.TVSources.Find(x => x.GroupTitle != null && x.GroupTitle.Equals(groupTitle));
        if (model != null)
        {
            appStorage.TVSources.Remove(model);
        }
    }

    public void LoadTVSources()
    {
        var models = JsonProvider.FromPathToObjects<TVSourceModel>(appSettingsConfig.TVSource);
        if (models != null && models.Count != 0)
        {
            appStorage.TVSources.Clear();
            TVSourceAddOrUpdate(models);
        }
    }

    public void TVSourceAddOrUpdate(IEnumerable<TVSourceModel> models)
    {
        appStorage.TVSources.AddRange(models);
    }
}
