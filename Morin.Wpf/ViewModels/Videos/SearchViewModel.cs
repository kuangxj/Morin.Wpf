﻿using AutoMapper;
using Morin.Services;
using Morin.Shared.Models;
using Morin.Shared.Parameters;
using Morin.Wpf.Adapters;
using Morin.Wpf.Common;
using Morin.Wpf.Messages;
using Morin.Wpf.ViewModels.Players;
using Stylet;
using StyletIoC;

namespace Morin.Wpf.ViewModels.Videos;

public class SearchViewModel(IContainer container,
    IAppService appService,
    ISourceProtocolAdapter sourceProtocolAdapter,
    IEventAggregator eventAggregator,
    IMapper mapper,
    IApiService apiService, IWindowManager windowManager) : Screen
{
    private readonly IContainer container = container;
    private readonly IAppService appService = appService;
    private readonly ISourceProtocolAdapter sourceProtocolAdapter = sourceProtocolAdapter;
    private readonly IEventAggregator eventAggregator = eventAggregator;
    private readonly IMapper mapper = mapper;
    private readonly IApiService apiService = apiService;
    private readonly IWindowManager windowManager = windowManager;
    private List<VideoModel> VideoList = [];
    public VideoModel VideoItem { get; set; }
    public BindableCollection<VideoModel> Videos { get; private set; } = [];
    private readonly static object video_Add_Lock = new();
    private Dictionary<int, MediaSourceModel> mediaSourceDict;
    public BindableCollection<MediaSourceModel> MediaSources { get; set; } = [];
    private MediaSourceModel _mediaSourceItem;

    public MediaSourceModel MediaSourceItem
    {
        get { return _mediaSourceItem; }
        set
        {
            _mediaSourceItem = value;
            MediaSourceItemChangedAsync(value);
        }
    }


    public string? KeyWord { get; set; }
    public int PageSize { get; set; } = 20;
    public int Total { get; set; } = 30;
    private int pageIndex;
    public int PageIndex
    {
        get => pageIndex; set
        {
            pageIndex = value;
            PageIndexChangedAsync(value);
        }
    }
    private Task PageIndexChangedAsync(int pageIndex)
    {
        return Task.Run(() =>
          {
              if (VideoList != null)
              {
                  Execute.PostToUIThreadAsync(() =>
                  {
                      if (PageIndex == 1)
                      {
                          Videos = [.. VideoList.Take(PageSize)];
                      }
                      else
                      {
                          Videos = [.. VideoList.Skip((PageIndex - 1) * PageSize).Take(PageSize)];
                      }
                  });
              }
          });
    }

    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();
        MediaSources = [.. appService.GetMediaSources()];
        mediaSourceDict = MediaSources.ToDictionary(x => x.Id, v => v);


        if (!string.IsNullOrEmpty(KeyWord))
        {
            TaskSearch(KeyWord);
        }


    }

    private Task MediaSourceItemChangedAsync(MediaSourceModel model)
    {
        return Task.Run(() =>
        {
            Execute.PostToUIThreadAsync(() =>
            {
                Videos = [.. VideoList.Where(x => x.VodSourceID == model.Id)];
            });
        });
    }

    private async void AddVideoData(int sourceID, string keyWord)
    {
        var req = new ReqQryVideoPara { SourceID = sourceID, KeyWord = keyWord, AcName = "videolist" };
        var rspData = await apiService.ReqQryVideosAsync(req);
        if (rspData != null && rspData.Videos != null && rspData.Videos.Count > 0)
        {
            rspData.Videos.ForEach(x =>
            {
                x.VodSourceID = sourceID;
                if (mediaSourceDict.TryGetValue(x.VodSourceID, out var model))
                {
                    x.VodSourceTitle = model.Title;
                }
            });
            lock (video_Add_Lock)
            {
                VideoList.AddRange(rspData.Videos);
                Total = VideoList.Count;
                if (Total <= PageSize)
                {
                    PageIndexChangedAsync(1);
                }
            }
        }
    }

    private void TaskSearch(string keyWord)
    {
        var cancellationSource = new CancellationTokenSource();
        var options = new ParallelOptions();
        // 最大并行度，并行的任务有几个
        options.MaxDegreeOfParallelism = 2;
        options.CancellationToken = cancellationSource.Token;

        if (mediaSourceDict != null && mediaSourceDict.Count > 0)
        {
            //  清空列表数据
            VideoList.Clear();
            //  并行查询
            var loopResult = Parallel.For(0, mediaSourceDict.Count, options, x =>
            {
                AddVideoData(mediaSourceDict.Keys.ElementAt(x), keyWord);
            });
        }
    }

    public Task PlayAsync(VideoModel o)
    {
        return Task.Run(async () =>
        {
            var req = new ReqQryVideoDetailPara { SourceID = o.VodSourceID, VodIds = $"{o.VodId}", AcName = "detail" };
            var detail = await apiService.ReqQryVideoDetailsAsync(req);
            Execute.PostToUIThread(() =>
            {
                var playVM = container.Get<PlayerViewModel>();

                //  播放列表
                var para = mapper.Map<ThinkPhpVideoParsingPara>(detail);
                playVM.PlayDict = sourceProtocolAdapter.GetPlayDict(para);
                windowManager.ShowWindow(playVM);
            });

        });
    }

    public void BackTo()
    {
        eventAggregator.Publish(new ScreenChangedMessage { ViewModel = "VideoViewModel" });
    }
}
