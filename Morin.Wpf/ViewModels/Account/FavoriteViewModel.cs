using Morin.Services;
using Morin.Shared.Models;
using Morin.Shared.Parameters;
using Morin.Wpf.Adapters;
using Morin.Wpf.Common;
using Morin.Wpf.ViewModels.Players;
using Morin.Wpf.Views.Account;
using Stylet;
using StyletIoC;
using System.Windows.Forms.Design;

namespace Morin.Wpf.ViewModels.Account;

internal class FavoriteViewModel(IContainer container, 
    ISourceProtocolAdapter sourceProtocolAdapter,
    IApiService apiService, IAppService appService, IWindowManager windowManager) : Screen
{

    private readonly IContainer? container = container;
    private readonly ISourceProtocolAdapter sourceProtocolAdapter = sourceProtocolAdapter;
    private readonly IApiService? apiService = apiService;
    private readonly IAppService? appService = appService;
    private readonly IWindowManager? windowManager = windowManager;

    #region 分页属性
    public int Total { get; set; }
    public int PageSize { get; set; } = 20;
    private int pageIndex = 1;
    public int PageIndex
    {
        get => pageIndex; set
        {
            pageIndex = value;
            PageIndexChangedAsync(value);
        }
    }

    #endregion
    private IEnumerable<FavoriteModel>? FavoriteEnumerable;
    public BindableCollection<FavoriteModel>? Favorites { get; set; }

    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();
        FavoriteEnumerable = appService?.GetFavorites();
        if (FavoriteEnumerable != null)
        {
            Total = FavoriteEnumerable.Count();
        }
        PageIndexChangedAsync(1);
    }
    public async void Play(FavoriteModel o)
    {
        var req = new ReqQryVideoDetailPara { SourceID = o.SourceID, VodIds = $"{o.VodId}", AcName = "detail" };
        var detail = await apiService?.ReqQryVideoDetailsAsync(req);

        var playVM = container?.Get<PlayerViewModel>();

        //  播放列表
        playVM.PlayDict = sourceProtocolAdapter.GetPlayDict(detail.VodPlayUrl,detail.VodPlayFrom);
        windowManager?.ShowWindow(playVM);
    }
    private Task PageIndexChangedAsync(int pageIndex)
    {
        return Task.Run(() =>
        {
            if (FavoriteEnumerable != null)
            {
                Execute.PostToUIThreadAsync(() =>
                {
                    if (PageIndex ==1)
                    {
                        Favorites = [.. FavoriteEnumerable.Take(PageSize)];
                    }
                    else
                    {
                        Favorites = [.. FavoriteEnumerable.Skip(PageIndex * PageSize).Take(PageSize)];
                    }
                });
            }
        });
    }
}
