using Morin.Shared.Configs;
using Morin.Shared.Models;

namespace Morin.Services;

public interface IAppService
{
    IEnumerable<ClassModel> GetClasses(int id);
    IEnumerable<MediaSourceModel> GetMediaSources();
    IEnumerable<MenuModel> GetMenus();
    IEnumerable<HistoryViewsModel> GetHistoryViews();
    IEnumerable<HistoryViewsModel> GetHistoryViews(int sourceID);
    IEnumerable<HistoryViewsModel> GetHistoryViews(int sourceID, int vodId);
    HistoryViewsModel GetHistoryView(string key);

    IEnumerable<HistorySearchModel> GetHistorySearchs();

    IEnumerable<PlaySkipTimeModel> GetPlaySkipTimes();
    IEnumerable<PlaySkipTimeModel> GetPlaySkipTimes(int sourceID, int vodId);
    PlaySkipTimeModel GetPlaySkipTime(string key);
    PlaySkipTimeModel GetPlaySkipTime(int sourceID, int vodId);

    void HistorySearchAddOrUpdate(HistorySearchModel model);
    void HistorySearchTryRemove(string key);

    void HistoryViewsAddOrUpdate(HistoryViewsModel historyView);
    void HistoryViewsTryRemove(string key);

    List<TVSourceModel> GetTVSources();
    void TVSourceAddOrUpdate(TVSourceModel model);
    void TVSourceAddOrUpdate(IEnumerable< TVSourceModel> models);
    void TVSourceTryRemove(string groupTitle);

    void MenusSave(IEnumerable<MenuModel> menus);

    void MediaSourcesTryRemove(int id);
    void MediaSourcesAddOrUpdate(MediaSourceModel media);

    void ClassesAddOrUpdate(int id, IEnumerable<ClassModel> classes);

    void PlaySkipTimesTryRmove(string key);
    void PlaySkipTimeAddOrUpdate(PlaySkipTimeModel skip);

    void LoadMediaSources();
    void LoadHistoryViews();
    void LoadPlaySkipTimes();
    void LoadHistorySearchs();
    void LoadTVSources();
    VideoSettingsConfig GetVideoSettings();
}
