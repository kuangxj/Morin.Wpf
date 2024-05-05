using Morin.Shared.Models;
using System.Collections.Concurrent;

namespace Morin.Storages;

public interface IAppStorage
{
    ConcurrentDictionary<int, IEnumerable<ClassModel>> ClasseDict { get; }
    ConcurrentDictionary<int, MediaSourceModel> MediaSourceDict { get; }
    IEnumerable<MenuModel> Menus { get; set; }
    ConcurrentDictionary<string, HistoryViewsModel> HistoryViewDict { get; set; }
    ConcurrentDictionary<string, PlaySkipTimeModel> PlaySkipTimeDict { get; set; }
    ConcurrentDictionary<string, HistorySearchModel> HistorySearchDict { get; set; }
    List<TVSourceModel> TVSources { get; set; }
}
