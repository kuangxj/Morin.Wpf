using AutoMapper;
using Morin.Services;
using Morin.Shared.Configs;
using Morin.Shared.Models;
using Morin.Shared.Parameters;
using Morin.Wpf.Adapters;
using Morin.Wpf.Common;
using Morin.Wpf.Messages.Players;
using Morin.Wpf.ViewModels.Players;
using Stylet;
using System.IO;
using IContainer = StyletIoC.IContainer;

namespace Morin.Wpf.ViewModels.Videos;

public class VideoViewModel(IApiService apiService,
    IContainer container,
    IWindowManager windowManager,
    IEventAggregator eventAggregator,
    IAppService appService,
    ISourceProtocolAdapter sourceProtocolAdapter,
    AppSettingsConfig appSettingsConfig,
    IMapper mapper) : Conductor<Screen>
{
    private readonly IApiService apiService = apiService;
    private readonly IContainer container = container;
    private readonly IWindowManager windowManager = windowManager;
    private readonly IEventAggregator eventAggregator = eventAggregator;
    private readonly IAppService appService = appService;
    private readonly ISourceProtocolAdapter sourceProtocolAdapter = sourceProtocolAdapter;
    private readonly AppSettingsConfig appSettingsConfig = appSettingsConfig;
    private readonly IMapper mapper = mapper;

    private ClassModel secondaryClassItem;
    public ClassModel SecondaryClassItem
    {
        get => secondaryClassItem; set
        {
            secondaryClassItem = value;
            ClassItem = value;
        }
    }

    private ClassModel primaryClassItem;
    public ClassModel PrimaryClassItem
    {
        get => primaryClassItem; set
        {
            primaryClassItem = value;
            if (value != null)
            {
                var classes = appService.GetClasses(MediaSourceItem.Id);
                if (classes.Count() > 0)
                {
                    SecondaryClasses = [.. classes.Where(x => x.Pid == value.Id)];

                    //  二级菜单为空的状态
                    if (SecondaryClasses.Count == 0)
                    {
                        ClassItem = value;
                    }
                }
            }
        }
    }

    private ClassModel classItem;
    public ClassModel ClassItem
    {
        get => classItem; set
        {
            classItem = value;
            OnClasseSelectedItemChanged(value);
        }
    }
    public BindableCollection<ClassModel> PrimaryClasses { get; set; }
    public BindableCollection<ClassModel> SecondaryClasses { get; set; }


    public BindableCollection<VideoModel> Videos { get; set; } = [];
    private VideoModel videoItem;
    public VideoModel VideoItem
    {
        get => videoItem;
        set
        {
            videoItem = value;
        }
    }


    public BindableCollection<MediaSourceModel> MediaSources { get; set; }
    private MediaSourceModel mediaSourceItem;
    public MediaSourceModel MediaSourceItem
    {
        get => mediaSourceItem;
        set
        {
            mediaSourceItem = value;

            PrimaryClasses = [];
            SecondaryClasses = [];

            OnMediaSourceItemChanged(value);

        }
    }

    public int PageSize { get; set; }

    public int Total { get; set; }


    private int pageIndex = 1;
    public int PageIndex
    {
        get => pageIndex; set
        {
            pageIndex = value;
            OnPageIndexChanged(value);
        }
    }




    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();
        CreateDirectories();

        appService.LoadMediaSources();
        appService.LoadHistoryViews();
        LoadMediaSources();
    }


    private void OnMediaSourceItemChanged(MediaSourceModel value)
    {
        eventAggregator.Publish(new OnMediaSourceItemChangedMessage { Model = value });

        Task.Run(async () =>
        {
            var classes = appService.GetClasses(value.Id);
            if (classes.Count() > 0)
            {

                //  主菜单
                PrimaryClasses = [.. classes.Where(x => x.Pid == 0)];
            }
            else
            {
                //  拉取采集源的分类列表
                var req = new ReqQryVideoPara { SourceID = value.Id, AcName = "list" };
                var rspData = await apiService.ReqQryVideosAsync(req);

                if (rspData.Classes != null)
                {
                    Execute.OnUIThread(() =>
                    {
                        //  采集源默认赋值
                        rspData.Classes.ForEach(c => c.SourceID = value.Id);

                        //  菜单列表
                        appService.ClassesAddOrUpdate(value.Id, rspData.Classes);

                        //  主菜单
                        PrimaryClasses = [.. rspData.Classes.Where(x => x.Pid == 0)];
                    });
                }
            }
        });
    }



    public async void Play(VideoModel o)
    {
        var req = new ReqQryVideoDetailPara { SourceID = o.SourceID, VodIds = $"{o.VodId}", AcName = "detail" };
        var detail = await apiService.ReqQryVideoDetailsAsync(req);

        var playVM = container.Get<PlayerViewModel>();
        //  播放列表
        playVM.PlayDict = sourceProtocolAdapter.GetPlayDict(detail.VodPlayUrl,detail.VodPlayFrom);
        windowManager.ShowWindow(playVM);
    }


    private void OnClasseSelectedItemChanged(ClassModel o)
    {
        if (o != null)
        {
            Task.Run(async () =>
            {
                //  拉取分类下视频列表
                var req = new ReqQryVideoPara { SourceID = o.SourceID, ClassID = o.Id, AcName = "videolist" };
                var rspData = await apiService.ReqQryVideosAsync(req);

                if (rspData != null)
                {
                    Videos?.Clear();

                    Total = rspData.Total;
                    PageSize = rspData.PageSize;
                    PageIndex = rspData.PageIndex;
                    Execute.OnUIThread(() =>
                    {
                        rspData.Videos.ForEach(x =>
                        {
                            x.SourceID = o.SourceID;
                            Videos.Add(mapper.Map<VideoModel>(x));
                        });
                    });
                }
            });
        }
    }

    private void OnPageIndexChanged(int pageIndex)
    {
        Task.Run(async () =>
         {
             var sourceID = ClassItem.SourceID;
             var clsID = ClassItem.Id;
             var req = new ReqQryVideoPara { SourceID = sourceID, ClassID = clsID, AcName = "detail", PageIndex = pageIndex };
             var rspData = await apiService.ReqQryVideosAsync(req);

             if (rspData != null)
             {
                 Execute.OnUIThread(() =>
                 {
                     Videos?.Clear();
                     rspData.Videos.ForEach(x =>
                     {
                         x.SourceID = sourceID;
                         Videos.Add(mapper.Map<VideoModel>(x));
                     });
                 });
             }
         });
    }

    private void LoadMediaSources()
    {
        var videoSources = appService.GetMediaSources();
        if (videoSources.Any())
        {
            MediaSources = [.. videoSources];
            //  默认选中
            if (MediaSources.Count > 0)
            {
                MediaSourceItem = MediaSources[0];
            }
        }
    }

    private void CreateDirectories()
    {
        if (!Directory.Exists(appSettingsConfig.PluginsPath))
        {
            Directory.CreateDirectory(appSettingsConfig.PluginsPath);
        }
        if (!Directory.Exists(appSettingsConfig.FFmpegPath))
        {
            Directory.CreateDirectory(appSettingsConfig.FFmpegPath);
        }
    }


}
