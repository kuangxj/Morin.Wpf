using Morin.Services;
using Morin.Shared.Models;
using Morin.Shared.Parameters;
using Morin.Wpf.Common;
using Morin.Wpf.ViewModels.Players;
using Stylet;
using StyletIoC;
using System.Windows.Forms.Design;

namespace Morin.Wpf.ViewModels.Account;

internal class HistoryViewModel(IContainer container, IApiService apiService, IAppService appService, IWindowManager windowManager) : Screen
{
    private readonly IContainer? container = container;
    private readonly IApiService? apiService = apiService;
    private readonly IAppService? appService = appService;
    private readonly IWindowManager? windowManager = windowManager;

    public BindableCollection<HistoryViewsModel>? HistoryViews { get; set; }
    #region 分页属性
    public int PageIndex { get; set; } = 1;
    public int Total { get; set; }
    public int PageSize { get; set; } = 20;
    #endregion
    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();

        HistoryViews = [.. appService?.GetHistoryViews()];

        Total = HistoryViews.Count;
    }
    public async void Play(VideoModel o)
    {
        var req = new ReqQryVideoDetailPara { SourceID = o.SourceID, Ids = $"{o.VodId}", AcName = "detail" };
        var detail = await apiService?.ReqQryVideoDetailsAsync(req);

        var playVM = container?.Get<PlayerViewModel>();

        //  播放列表
        playVM.PlayDict = VideoUriToEspode.FromUrlToEspodes(detail);
        windowManager?.ShowWindow(playVM);
    }
}
