using MaterialDesignThemes.Wpf;
using Morin.Services;
using Morin.Shared.Common;
using Morin.Shared.Configs;
using Morin.Shared.Models;
using Stylet;
using System.ComponentModel;
using System.Windows.Data;

namespace Morin.Wpf.ViewModels.Settings;

public class VideoSettingsViewModel(IAppService appService,  AppSettingsConfig appSettingsConfig) : Screen
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
        if (e.Parameter != null && e.Parameter.Equals(true))
        {
            var eq = appService.GetMediaSources().FirstOrDefault(x => !string.IsNullOrEmpty(x.Title) && x.Title.Equals(Title));
            if (eq == null)
            {
                //  添加
                var id = appService.GetMediaSources().Max(x => x.Id);
                var model = new MediaSourceModel(id++)
                {
                    JsonUri = JsonUri,
                    ParsingUri = ParsingUri,
                    Title = Title,
                    XmlUri = XmlUri
                };
                MediaSources.Add(model);
                //  更新
                appService.MediaSourcesAddOrUpdate(model);

                JsonUri = null;
                ParsingUri = null;
                Title = null;
                XmlUri = null;

                Search();

            }


        }

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

