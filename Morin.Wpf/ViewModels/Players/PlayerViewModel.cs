using FlyleafLib;
using FlyleafLib.MediaPlayer;
using MaterialDesignThemes.Wpf;
using Morin.Services;
using Morin.Shared.Configs;
using Morin.Shared.Models;
using Morin.Wpf.Messages.Players;
using Stylet;
using StyletIoC;
using System.Windows;

namespace Morin.Wpf.ViewModels.Players;

public class PlayerViewModel(IEventAggregator eventAggregator,
    AppSettingsConfig appSettingsConfig,
    IAppService appService,
    IContainer container,
    ISnackbarMessageQueue snackbarMessageQueue) :
    Conductor<Screen>,
    IDisposable,
    IHandle<OnEpisodeChangedMessage>
{
    private readonly IEventAggregator eventAggregator = eventAggregator;
    private readonly AppSettingsConfig appSettingsConfig = appSettingsConfig;
    private readonly IAppService appService = appService;
    private readonly IContainer container = container;
    private readonly ISnackbarMessageQueue snackbarMessageQueue = snackbarMessageQueue;
    public WindowState WindowState { get; set; }
    public bool IsPlay { get; set; }
    private VideoSettingsConfig videoSettings;
    private List<PlaySkipTimeModel> playSkipTimes;  
    public Dictionary<string, List<VideoModel>> PlayDict { get; set; }
    public VideoModel VideoItem { get; set; }
    public int PlayerListWidth { get; set; } = 300;
    public string Title { get; set; }
    public Player Player { get; set; }
    public Config Config { get; set; }
    public ISnackbarMessageQueue MessageQueue { get; } = snackbarMessageQueue;
    public bool ToClose { get; set; }
    public PlayMode PlayMode { get; set; } = PlayMode.Video;
    private int skipBeginPosition;
    public int SkipBeginPosition
    {
        get => skipBeginPosition; set
        {

            skipBeginPosition = value;

            SkipTimeUpdate();
            SkipBegin();
        }
    }

    private int skipEndPosition;
    public int SkipEndPosition
    {
        get => skipEndPosition; set
        {

            skipEndPosition = value;
            SkipTimeUpdate();

            SkipEnd();

        }
    }
    private bool skipTimeSwitch;
    public bool SkipTimeSwitch
    {
        get => skipTimeSwitch; set
        {
            skipTimeSwitch = value;
            SkipTimeUpdate();
        }
    }
    private VideoModel curVideo;

    public PlayerListViewModel? PlayerListView { get; set; }

    public void Handle(OnEpisodeChangedMessage message)
    {
        if (message.Model != null && message.Model.VodPlayUrl.EndsWith("m3u8"))
        {
            curVideo = message.Model;
            Player.OpenAsync(message.Model.VodPlayUrl);
        }
    }
  

    private void InitializeComponent()
    {
        // Initializes Engine (Specifies FFmpeg libraries path which is required)
        Engine.Start(new EngineConfig()
        {
#if DEBUG
            LogOutput = ":debug",
            LogLevel = LogLevel.Debug,
            FFmpegLogLevel = FFmpegLogLevel.Warning,
#endif

            PluginsPath = $":{appSettingsConfig.PluginsPath}",
            FFmpegPath = $":{appSettingsConfig.FFmpegPath}",

            // Use UIRefresh to update Stats/BufferDuration (and CurTime more frequently than a second)
            UIRefresh = true,
            UIRefreshInterval = 100,
            UICurTimePerSecond = false // If set to true it updates when the actual timestamps second change rather than a fixed interval
        });

        Config = DefaultConfig();

        // Inform the lib to refresh stats
        Config.Player.Stats = true;

        Player = new Player(Config);
        Player.Audio.Volume = videoSettings.Volume;

        //  声音属性变化
        Player.Audio.PropertyChanged += Player_Audio_PropertyChanged;
        //  视频进度属性变化
        Player.PropertyChanged += Player_PropertyChanged;

        // Keep track of error messages
        Player.OpenCompleted += Player_OpenCompleted;
        Player.BufferingCompleted += Player_BufferingCompleted;

    }

    private void Player_BufferingCompleted(object? s, BufferingCompletedArgs e)
    {
        if (!string.IsNullOrEmpty(e.Error))
        {
            MessageQueue.Enqueue(e.Error);
        }
    }

    private void Player_Audio_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals("Volume"))
        {
            if (sender is Audio audio && videoSettings != null)
            {
                videoSettings.Volume = audio.Volume;
            }
        }
    }

    private void Player_OpenCompleted(object? sender, OpenCompletedArgs e)
    {
        IsPlay = true;

        if (!string.IsNullOrEmpty(e.Error))
        {
            MessageQueue.Enqueue(e.Error);
        }
        var skipTime = appService.GetPlaySkipTime(curVideo.SourceID, curVideo.VodId);
        if (skipTime != null)
        {
            SkipTimeSwitch = skipTime.SkipSwitch;
            SkipBeginPosition = skipTime.SkipBeginPosition;
            SkipEndPosition = skipTime.SkipEndPosition;

            SkipBegin();
        }
    }

    private void Player_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals("CurTime"))
        {
            if (sender is Player player)
            {
                //  跳转开关：开的情况
                if (SkipTimeSwitch)
                {
                    //  满足条件就结尾
                    SkipEnd();
                }
            }
        }
    }

    private Config DefaultConfig()
    {
        var config = new Config();

        // Mainly for HLS to pass the original query which might includes session keys       
        config.Demuxer.FormatOptToUnderlying = true;
        //  加大缓存进度
        config.Demuxer.BufferDuration = 300 *(long)1000 * 10000;
        // To allow embedded atempo filter for speed
        config.Audio.FiltersEnabled = true;

        // Set it empty so it will include it when we save it
        config.Video.GPUAdapter = "";       

        config.Subtitles.SearchLocal = true;  
        return config;
    }

    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();

        appService.LoadHistoryViews();
        appService.LoadPlaySkipTimes();

        videoSettings = appService.GetVideoSettings();
        playSkipTimes = [.. appService.GetPlaySkipTimes()];

        InitializeComponent();

        InitPlayListView();

    }
    private void InitPlayListView()
    {
        PlayerListView = container.Get<PlayerListViewModel>();
        PlayerListView.PropertyChanged -= PlayerListView_PropertyChanged;
        PlayerListView.PlayDict = PlayDict;
        PlayerListView.PlayMode=PlayMode;
        PlayerListView.VideosSortByAsc = videoSettings.VideosSortByAsc;
        ActiveItem = PlayerListView;

        PlayerListView.PropertyChanged += PlayerListView_PropertyChanged;
    }

    private void PlayerListView_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals("VideosSortByAsc"))
        {
            if (sender is PlayerListViewModel playerList)
            {
                videoSettings.VideosSortByAsc = playerList.VideosSortByAsc;
            }
        }
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        eventAggregator.Subscribe(this);
    }
    protected override void OnDeactivate()
    {
        base.OnDeactivate();
        eventAggregator.Unsubscribe(this);
    }

    public void ExpendAll()
    {
        PlayerListWidth = PlayerListWidth == 300 ? 0 : 300;
    }
    public void WindowClose()
    {
        videoSettings.Save(appSettingsConfig.VideoConfig);

        if (!Player.IsDisposed)
        {
            Player.Dispose();
        }
        ToClose = true;
    }
    public void WindowMinimize()
    {
        WindowState = WindowState.Minimized;
    }
    public void NavigateBefore()
    {
        if (PlayerListView != null)
        {
            PlayerListView.NavigateBefore();
        }
    }
    public void NavigateNext()
    {
        if (PlayerListView != null)
        {
            PlayerListView.NavigateNext();
        }
    }
    private void SkipTimeUpdate()
    {
        var skip = new PlaySkipTimeModel
        {
            SkipBeginPosition = SkipBeginPosition,
            SkipEndPosition = SkipEndPosition,
            SourceID = curVideo.SourceID,
            VodId = curVideo.VodId,
            SkipSwitch = SkipTimeSwitch
        };
        appService.PlaySkipTimeAddOrUpdate(skip);
    }
    private void SkipBegin()
    {
        //  直接跳转
        var begin = TimeSpan.FromSeconds(SkipBeginPosition).Ticks;
        if (Player.IsPlaying && Player.CurTime < begin)
        {
            Player.CurTime = begin;
        }
    }
    private void SkipEnd()
    {
        //  直接跳转
        var end = TimeSpan.FromSeconds(SkipEndPosition).Ticks;
        if (Player.IsPlaying && (Player.Duration - Player.CurTime) <= end)
        {
            NavigateNext();
        }
    }
    public void Dispose()
    {
        if (!Player.IsDisposed)
        {
            Player.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}
