using AutoMapper;
using Morin.Services;
using Morin.Shared.Models;
using Morin.Shared.Parameters;
using Morin.Wpf.Adapters;
using Morin.Wpf.Common;
using Morin.Wpf.ViewModels.Players;
using Newtonsoft.Json.Linq;
using Stylet;
using StyletIoC;
using System.Windows.Forms.Design;

namespace Morin.Wpf.ViewModels.Account;

internal class HistoryViewModel(IContainer container, 
    IApiService apiService, 
    ISourceProtocolAdapter sourceProtocolAdapter,
    IAppService appService,
    IWindowManager windowManager) : Screen
{
    private readonly IContainer? container = container;
    private readonly IApiService? apiService = apiService;
    private readonly ISourceProtocolAdapter sourceProtocolAdapter = sourceProtocolAdapter;
    private readonly IAppService? appService = appService;
    private readonly IWindowManager? windowManager = windowManager;

    private IEnumerable<HistoryViewsModel>? HistoryViewEnumerable;
    public BindableCollection<HistoryViewsModel>? HistoryViews { get; set; }
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
    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();

        HistoryViewEnumerable = appService?.GetHistoryViews();
        if (HistoryViewEnumerable != null)
        {
            Total = HistoryViewEnumerable.Count();
        }

        PageIndexChangedAsync(1);
    }
    public async void Play(HistoryViewsModel o)
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
            if (HistoryViewEnumerable != null)
            {
                Execute.PostToUIThreadAsync(() =>
                {
                    if (PageIndex ==1)
                    {
                        HistoryViews = [.. HistoryViewEnumerable.Take(PageSize)];
                    }
                    else
                    {
                        HistoryViews = [.. HistoryViewEnumerable.Skip(PageIndex * PageSize).Take(PageSize)];
                    }
                });
            }
        });
    }
}
