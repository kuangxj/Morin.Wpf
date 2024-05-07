using FlyleafLib.MediaFramework.MediaPlaylist;
using Morin.Services;
using Morin.Shared.Models;
using Morin.Shared.Parameters;
using Morin.Wpf.Common;
using Morin.Wpf.ViewModels.Players;
using Stylet;
using StyletIoC;
using System.Reflection;
using System.Windows.Controls;

namespace Morin.Wpf.ViewModels.Television;

public class TelevisionViewModel(IAppService appService, IContainer container, IWindowManager windowManager) : Screen
{
    private readonly IAppService appService = appService;
    private readonly IContainer container = container;
    private readonly IWindowManager windowManager = windowManager;

    public string[]? TVGroupTitles { get; set; }
    private string? _tVGroupTitle;

    public string? TVGroupTitle
    {
        get { return _tVGroupTitle; }
        set
        {
            _tVGroupTitle = value;
            if (!string.IsNullOrEmpty(value))
            {
                var model = TVSources.Find(x => x.GroupTitle != null && x.GroupTitle.Equals(value));
                if (model != null)
                {
                    TVSourceDetails = [.. model.TVSourceDetails];
                    Total = TVSourceDetails.Count;
                }
            }
        }
    }
    public int PageSize { get; set; } = 20;
    public int Total { get; set; }
    public int PageIndex { get; set; }

    private List<TVSourceModel> TVSources;
    public BindableCollection<TVSourceDetailModel>? TVSourceDetails { get; set; }
    public TVSourceModel? TVSourceDetailItem { get; set; }

    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();

        TVSources = appService.GetTVSources();

        TVGroupTitles = [.. TVSources.Select(x => x.GroupTitle)];
        if (TVGroupTitles != null && TVGroupTitles.Length > 0)
        {
            TVGroupTitle = TVGroupTitles[0];
        }
    }

    private string GetEnumDescription(Enum enumObj)
    {
        FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
        if (fieldInfo != null)
        {
            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                if (attribArray[0] is System.ComponentModel.DescriptionAttribute attrib)
                {
                    return attrib.Description;
                }
                return enumObj.ToString();
            }
        }
        return enumObj.ToString();
    }

    public void Play(TVSourceDetailModel o)
    {
        var playDict = new Dictionary<string, IEnumerable<VideoModel>>();
        var videoList = new List<VideoModel>();
        var model = new VideoModel { Episode = "1", VodPlayUrl = o.WebAddr };
        videoList.Add(model);

        var key = GetEnumDescription(o.NetworkCarrierType);
        playDict.Add(key, videoList);



        var viewModel = container.Get<PlayerViewModel>();
        viewModel.PlayMode = PlayMode.TV;
        viewModel.PlayDict = playDict;
        windowManager.ShowWindow(viewModel);

    }
}
