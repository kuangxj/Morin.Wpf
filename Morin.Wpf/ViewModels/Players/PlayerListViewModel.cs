using FlyleafLib.MediaFramework.MediaPlaylist;
using Morin.Services;
using Morin.Shared.Models;
using Morin.Wpf.Messages.Players;
using Stylet;

namespace Morin.Wpf.ViewModels.Players;

public class PlayerListViewModel(IAppService appService, IEventAggregator eventAggregator) : Screen
{
    public PlayMode PlayMode { get; set; } = PlayMode.Video;
    public BindableCollection<VideoModel> Videos { get; set; }
    private readonly IAppService appService = appService;
    private readonly IEventAggregator eventAggregator = eventAggregator;
    private bool videosSortByAsc;
    public bool VideosSortByAsc
    {
        get => videosSortByAsc; set
        {
            videosSortByAsc = value;
            //  倒序显示
            if (PlayList != null)
            {
                PlayList = !value ? PlayList.OrderByDescending(x => x.Sort).ToList() : PlayList.OrderBy(x => x.Sort).ToList();
                Videos = [.. PlayList];
            }
        }
    }

    /// <summary>
    /// 播放清单
    /// </summary>
    public List<VideoModel> PlayList { get; set; }
    private int playPosition;


    private VideoModel videoItem;
    public VideoModel VideoItem
    {
        get => videoItem; set
        {
            videoItem = value;
            if (value != null)
            {
                //  更新历史观看
                var history = new HistoryViewsModel
                {
                    Episode = value.Episode,
                    SourceID = value.SourceID,
                    VodId = value.VodId,
                };
                appService.HistoryViewsAddOrUpdate(history);

                //  当前播放下标
                playPosition = PlayList.IndexOf(value);

                //  通知播放器播放
                EpisodeChanged(value);
            }
        }
    }

    private string line;
    public string Line
    {
        get => line; set
        {
            line = value;
            if (PlayMode == PlayMode.Video)
            {
                var key = LineDict.FirstOrDefault(x => x.Value.Equals(line)).Key;
                if (!string.IsNullOrEmpty(key))
                {
                    //  倒序显示
                    PlayList = !VideosSortByAsc ? PlayDict[key].OrderByDescending(x => x.Sort).ToList() : PlayDict[key].OrderBy(x => x.Sort).ToList();
                    Videos = [.. PlayList];

                    //  设置最后观看
                    var historyViews = appService.GetHistoryViews(Videos[0].SourceID, Videos[0].VodId);
                    VideoItem = HistoryToLastEpisode(PlayList, historyViews);
                }
            }
            else
            {
                PlayList = PlayDict[value];
                VideoItem = PlayList.FirstOrDefault();
            }
        }
    }
    private VideoModel HistoryToLastEpisode(IEnumerable<VideoModel> videos, IEnumerable<HistoryViewsModel> historyViews)
    {
        if (!historyViews.Any()) return videos.First();

        var lastViewModel = historyViews.MaxBy(x => x.ViewTime);
        return videos.FirstOrDefault(x => x.SourceID == lastViewModel.SourceID && x.VodId == x.VodId && x.Episode.Equals(lastViewModel.Episode)) ?? videos.First();
    }

    public List<string> Lines { get; set; }
    private Dictionary<string, string> LineDict = [];

    public Dictionary<string, List<VideoModel>> PlayDict { get; set; } = [];

    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();
        //  播放视频
        switch (PlayMode)
        {
            case PlayMode.Video:
                SetVideoLineInfoAsync();
                break;
            case PlayMode.TV:
                SetTVLineInfoAsync();
                break;
            case PlayMode.LIVE:
                break;
            default:
                break;
        }
    }
    private Task SetTVLineInfoAsync()
    {
        return Task.Run(() =>
        {
            Lines = [.. PlayDict.Keys];

            //  默认显示 
            if (Lines.Count > 0)
            {
                Line = Lines[0];
            }

        });
    }

    private Task SetVideoLineInfoAsync()
    {
        return Task.Run(() =>
          {
              string[] lines = { "线路一", "线路二", "线路三", "线路四" };
              for (int i = 0; i < PlayDict.Keys.Count; i++)
              {
                  LineDict.Add(PlayDict.Keys.ElementAt(i), lines[i]);
              }
              //  只显示m3u8协议链接
              //  线路一没有解析，直接选m3u8模式的
              var m3u8 = LineDict.Keys.FirstOrDefault(x => x.Contains("m3u8"));
              if (!string.IsNullOrEmpty(m3u8))
              {
                  Lines = [LineDict[m3u8]];

                  //  默认显示 
                  if (Lines.Count > 0)
                  {
                      Line = Lines[0];
                  }
              }
          });
    }

    public void EpisodeChanged(VideoModel videoItem)
    {
        eventAggregator.Publish(new OnEpisodeChangedMessage { Model = videoItem });
    }
    public void NavigateBefore()
    {
        SetPlayPosition(playPosition, true, VideosSortByAsc);
    }
    public void NavigateNext()
    {
        SetPlayPosition(playPosition, false, VideosSortByAsc);
    }

    private void SetPlayPosition(int index, bool beforeOrNext, bool asc)
    {
        if (PlayList != null && PlayList.Count > 0)
        {
            if (asc && beforeOrNext) //  倒序，下一个
            {
                index--;
                if (index < 0)
                {
                    index = 0;
                }
            }
            else if (!asc && beforeOrNext) //  正序，下一个
            {

                index++;
                if (index == PlayList.Count)
                {
                    index = PlayList.Count - 1;
                }
            }
            else if (asc && !beforeOrNext)//  倒序，上一个
            {
                index++;
                if (index == PlayList.Count)
                {
                    index = PlayList.Count - 1;
                }
            }
            else if (!asc && !beforeOrNext)//  正序，上一个
            {

                index--;
                if (index < 0)
                {
                    index = 0;
                }
            }
            //  重复的操作
            if (VideoItem == PlayList[index])
            {
                VideoItem = PlayList[index];
            }
        }
    }
}
