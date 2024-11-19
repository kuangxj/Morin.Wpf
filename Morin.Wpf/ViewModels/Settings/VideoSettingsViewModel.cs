using MaterialDesignThemes.Wpf;
using Morin.Services;
using Morin.Shared.Common;
using Morin.Shared.Configs;
using Morin.Shared.Models;
using Stylet;
using System.ComponentModel;
using System.Windows.Data;

namespace Morin.Wpf.ViewModels.Settings;

public class VideoSettingsViewModel(IAppService appService, AppSettingsConfig appSettingsConfig) : Screen
{

    private MediaSourceModel mediaSourceModelItem;

    private readonly IAppService appService = appService;
    private readonly AppSettingsConfig appSettingsConfig = appSettingsConfig;

    public MediaSourceModel MediaSourceItem { get => mediaSourceModelItem; set => mediaSourceModelItem = value; }
    public ICollectionView MediaCollectionView { get; set; }
    public string SearchContent { get; set; }
    public BindableCollection<MediaSourceModel> MediaSources { get; set; }
    public string? Title { get; set; }
    public string? JsonUri { get; set; }
    public string? XmlUri { get; set; }
    public string? ParsingUri { get; set; }

    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();

        var mediaSources = appService.GetMediaSources();

        MediaSources = [.. mediaSources];
        MediaCollectionView = CollectionViewSource.GetDefaultView(MediaSources);
        MediaCollectionView.Filter = (o) =>
        {
            if (string.IsNullOrEmpty(SearchContent)) return true;
            if (o is MediaSourceModel s)
            {
                return !string.IsNullOrEmpty(s.Title) && s.Title.Contains(SearchContent);
            }
            return true;
        };

    }

    public void Search()
    {
        MediaCollectionView?.Refresh();
    }
    public void OnDialogClosing(object sender, DialogClosingEventArgs e)
    {
        if (e.Parameter == null) return;
        if (e.Parameter.Equals(false)) return;
        if (string.IsNullOrEmpty(Title)) return;

        //  构建实体
        var model = new MediaSourceModel
        {
            JsonUri = JsonUri,
            ParsingUri = ParsingUri,
            Title = Title,
            XmlUri = XmlUri
        };

        var videoSources = appService.GetMediaSources();
        //  1、数据库中无数据，直接增加；
        //  2、数据库有数据就找相同名称的，有就更新，没有就新增；
        if (videoSources == null || videoSources.Count() == 0)
        {
            model.Id = 1;
            MediaSources.Add(model);
        }
        else
        {
            var videoSource = videoSources.FirstOrDefault(x => x.Title.Equals(Title));
            if (videoSource != null)
            {
                //  此处不更新Id
                videoSource.Title = model.Title;
                videoSource.JsonUri = model.JsonUri;
                videoSource.XmlUri = model.XmlUri;
                videoSource.ParsingUri = model.ParsingUri;
                //  更新
                appService.MediaSourcesAddOrUpdate(videoSource);
            }
            else
            {
                //  新增
                var maxId = videoSources.Max(x => x.Id);
                model.Id = maxId + 1;
                appService.MediaSourcesAddOrUpdate(model);
            }

        }

        JsonUri = null;
        ParsingUri = null;
        Title = null;
        XmlUri = null;

        //  触发查询任务
        Search();

    }


    public void Remove(MediaSourceModel value)
    {
        MediaSources.Remove(value);
        appService.MediaSourcesTryRemove(value.Id);
    }

    public void DataSynchronous(MediaSourceModel sourceModel)
    {
        Task.Run(async () =>
        {
            if (!string.IsNullOrEmpty(sourceModel.JsonUri))
            {
                var rspData = await HttpsProvider.GetAsync(sourceModel.JsonUri);
                if (rspData != null)
                {
                    var jsonModel = JsonProvider.FromContentToObject<RspVideoListJsonDataModel>(rspData);
                    if (jsonModel != null)
                    {
                        sourceModel.Total = jsonModel.Total;
                    }
                    else
                    {
                        // sourceModel.Total = rspData.Total;
                    }
                }
            }
            else
            {

                var rspData = await HttpsProvider.GetAsync(sourceModel.XmlUri);
                if (rspData != null)
                {
                    var xmlModel = XmlProvider.DESerializer<RspVideoListXmlRootModel>(rspData);
                    sourceModel.Total = int.Parse(xmlModel.Videos.Total);
                }
            }
            //  缓存
            foreach (var item in MediaSources)
            {
                appService.MediaSourcesAddOrUpdate(item);
            }
        });
    }
}

