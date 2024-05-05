using MaterialDesignThemes.Wpf;
using Morin.Services;
using Morin.Shared.Models;
using Stylet;
using System.ComponentModel;
using System.Windows.Data;


namespace Morin.Wpf.ViewModels.Settings;

internal class TelevisionSettingsViewModel(IAppService appService) : Screen
{
    private readonly IAppService appService = appService;



    /// <summary>
    /// 通信运营商类型
    /// </summary>
    public static IEnumerable<NetworkCarrierType> NetworkCarrierTypes => Enum.GetValues(typeof(NetworkCarrierType)).Cast<NetworkCarrierType>();
    private NetworkCarrierType _networkCarrierType;
    public NetworkCarrierType NetworkCarrierType
    {
        get { return _networkCarrierType; }
        set { _networkCarrierType = value; }
    }

    private List<TVSourceModel>? _tVSources;
    private TVSourceModel? _tVSourceItem;
    public TVSourceModel? TVSourceItem
    {
        get { return _tVSourceItem; }
        set
        {
            _tVSourceItem = value;
            if (value != null)
            {
                TVSourceDetails = [.. value.TVSourceDetails];
            }
        }
    }
    public string? TVSourceGroupTitle { get; set; }
    public string? TVSourceDetailTitle { get; set; }
    public string? WebAddr { get; set; }

    public string? SearchContent { get; set; }
    public ICollectionView? TVCollectionView { get; set; }

    public BindableCollection<TVSourceDetailModel> TVSourceDetails { get; set; } = [];
    private TVSourceDetailModel _networkCarrier = new();
    public TVSourceDetailModel NetworkCarrier
    {
        get { return _networkCarrier; }
        set { _networkCarrier = value; }
    }



    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();

        _tVSources = [.. appService.GetTVSources()];

        TVCollectionView = CollectionViewSource.GetDefaultView(_tVSources);
        TVCollectionView.Filter = OnFilter;

    }
    private bool OnFilter(object o)
    {
        var result = false;
        if (o is TVSourceModel model)
        {
            if (string.IsNullOrEmpty(SearchContent)) return true;
            result = !string.IsNullOrEmpty(SearchContent)
                && !string.IsNullOrEmpty(model.GroupTitle)
                && model.GroupTitle.Contains(SearchContent);
        }
        return result;
    }

    public void Search()
    {
        TVCollectionView?.Refresh();
    }

    public void TVSourceOnDialogClosing(object sender, DialogClosingEventArgs e)
    {
        if (e.Parameter != null && e.Parameter.Equals(true))
        {
            var source = _tVSources?.FirstOrDefault(x => x.GroupTitle != null
            && x.GroupTitle.Equals(TVSourceGroupTitle));

            if (source == null)
            {
                var model = new TVSourceModel
                {
                    GroupTitle = TVSourceGroupTitle,
                    TVSourceDetails = []
                };
                appService.TVSourceAddOrUpdate(model);
                _tVSources?.Add(model);

                TVSourceGroupTitle = null;

                Search();
            }
        }
    }

    public void TVSourceDetailOnDialogClosing(object sender, DialogClosingEventArgs e)
    {
        if (e.Parameter != null && e.Parameter.Equals(true))
        {
            if (TVSourceItem != null)
            {
                //  添加
                var networkCarrier = new TVSourceDetailModel
                {
                    Title=TVSourceDetailTitle,
                    WebAddr = WebAddr,
                    NetworkCarrierType = this.NetworkCarrierType
                };
                TVSourceDetails.Add(networkCarrier);
                TVSourceItem.TVSourceDetails = TVSourceDetails;
                //  更新
                appService.TVSourceAddOrUpdate(TVSourceItem);

                WebAddr = null;
                TVSourceDetailTitle = null;
            }
        }

    }
    public void TVSourceDetailsRemove(TVSourceDetailModel networkCarrier)
    {
        TVSourceDetails.Remove(networkCarrier);
        //  更新
        if (TVSourceItem!=null)
        {
            TVSourceItem.TVSourceDetails = TVSourceDetails;
            appService.TVSourceAddOrUpdate(TVSourceItem);
        }
    }
    public void TVSourceRemove(TVSourceModel model)
    {
        if (model != null && !string.IsNullOrEmpty(model.GroupTitle))
        {
            appService.TVSourceTryRemove(model.GroupTitle);
            _tVSources?.Remove(model);
        }
    }
}
