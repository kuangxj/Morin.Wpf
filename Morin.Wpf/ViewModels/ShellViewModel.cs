
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Morin.Services;
using Morin.Shared.Models;
using Morin.Wpf.Messages;
using Morin.Wpf.Messages.Players;
using Morin.Wpf.ViewModels.Videos;
using Stylet;
using StyletIoC;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Morin.Wpf.ViewModels;


public class ShellViewModel(IConfiguration configuration, IContainer container,
    IAppService appService, IEventAggregator eventAggregator)
    : Conductor<Screen>.Collection.OneActive,
    IHandle<WindowStateChangedMessage>,
    IHandle<ScreenChangedMessage>
{
    private readonly IConfiguration configuration = configuration;
    private readonly IContainer container = container;
    private readonly IAppService appService = appService;
    private readonly IEventAggregator eventAggregator = eventAggregator;

    public string Title { get; set; } = "Morin";
    public bool HistorySearchPanelIsOpen { get; set; }
    private DispatcherTimer historySearchPanelDispatcherTimer;

    public BindableCollection<MenuModel> Menus { get; set; }
    private HistorySearchModel historySearch;

    public HistorySearchModel HistorySearchItem
    {
        get { return historySearch; }
        set
        {
            historySearch = value;
            if (value != null && !string.IsNullOrEmpty(value.KeyWord))
            {
                KeyWord = value.KeyWord;
                VideoSearch(KeyWord);
            }
        }
    }

    public BindableCollection<HistorySearchModel> HistorySearchs { get; set; }

    public WindowState WindowState { get; set; }     

    private bool isDarkTheme = true;
    public bool IsDarkTheme

    {
        get => isDarkTheme;
        set
        {
            isDarkTheme = value;
            OnThemeChangedAsync(value);
        }
    }

    private MenuModel menuItem;
    public MenuModel MenuItem
    {
        get => menuItem;
        set
        {
            menuItem = value;
            OnMenuItemChanged(value);
        }
    }

    public string KeyWord { get; set; }


    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();

        eventAggregator.Subscribe(this);       

        LoadDefaultData();

        //  查询面板定时器
        historySearchPanelDispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
        historySearchPanelDispatcherTimer.Tick += ClosePopupTimer_Tick;
    }

    private void ClosePopupTimer_Tick(object? sender, EventArgs e)
    {
        HistorySearchPanelIsOpen = false;
    }

    private void LoadDefaultData()
    {
        //  加载来源
        appService.LoadMediaSources();
        //  加载历史
        appService.LoadHistorySearchs();
        //  加载电视设置
        appService.LoadTVSources();
        //  加载历史观看
        appService?.LoadHistoryViews();
        //  加载收藏
        appService?.LoadFavorites();
        //  加载跳过【开始、结尾】
        appService?.LoadPlaySkipTimes();

        HistorySearchs = [.. appService?.GetHistorySearchs()];

        //  菜单设置
        var menus = CreateMenuBars();
        appService?.MenusSave(menus);

        //  加载菜单
        Menus = [.. menus.Where(x => x.Pid == 0&&x.Visvisibility==true)];
        MenuItem = Menus.First(x => x.Selected == true);


    }


    private MenuModel[] CreateMenuBars()
    {
        return [
            new MenuModel { Id = 1, ViewModel = "VideoViewModel", Icon = "MovieOutline", Pid = 0, Selected = true, Sort = 0, Title = "视频" },
            new MenuModel { Id = 2, ViewModel = "TelevisionViewModel", Icon = "TelevisionClassic", Pid = 0, Selected = false, Sort = 0, Title = "电视" },
            new MenuModel { Id = 3, ViewModel = "SettingsViewModel", Icon = "SettingsOutline", Pid = 0,Visvisibility=false, Selected = false, Sort = 0, Title = "设置" },//  不显示 
            new MenuModel { Id = 4, ViewModel = "AccountViewModel", Icon = "AccountWrenchOutline", Pid = 0, Selected = false, Sort = 0, Title = "我的" },
            new MenuModel { Id = 5, ViewModel = "VideoSettingsViewModel", Icon = "MovieOutline", Pid = 3, Selected = false, Sort = 0, Title = "视频设置" },
            new MenuModel { Id = 6, ViewModel = "TelevisionSettingsViewModel", Icon = "TelevisionClassic", Pid = 3, Selected = false, Sort = 0, Title = "电视设置" },
            new MenuModel { Id = 7, ViewModel = "AboutViewModel", Icon = "AboutOutline", Pid = 3, Selected = false, Sort = 0, Title = "关于Morin" },
            new MenuModel { Id = 8, ViewModel = "FavoriteViewModel", Icon = "HeartOutline", Pid = 4, Selected = false, Sort = 0, Title = "收藏" },
            new MenuModel { Id = 9, ViewModel = "HistoryViewModel", Icon = "History", Pid = 4, Selected = false, Sort = 0, Title = "历史" },
        ];
    }
    public void AppSettings()
    {
        ActiveItem = GetScreen("SettingsViewModel");
    }

    public void MinimizeWindow()
    {
        if (WindowState != WindowState.Minimized)
        {
            WindowState = WindowState.Minimized;
        }
    }

    public void MaximizeWindow()
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    public void WindowClose()
    {
        App.Current.Shutdown();
    }


    public void RestoreWindow()
    {
        if (WindowState != WindowState.Normal)
        {
            WindowState = WindowState.Normal;
        }
    }
    private void OnMenuItemChanged(MenuModel value)
    {
        ActivateItem(GetScreen(value.ViewModel));
    }
    private void OnThemeChangedAsync(bool value)
    {
        Task.Run(() =>
       {
           var paletteHelper = new PaletteHelper();
           var theme = paletteHelper.GetTheme();

           theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);
           paletteHelper.SetTheme(theme);
       });

    }


    public void Handle(ScreenChangedMessage message)
    {
        ActivateItem(GetScreen(message.ViewModel));
    }
    public void Handle(WindowStateChangedMessage message)
    {
        WindowState = message.IsFullScreen ? WindowState.Maximized : WindowState.Normal;
    }

    public void VideoSearch(string keyWord)
    {
        if (string.IsNullOrEmpty(keyWord)) return;
        //  隐藏历史查询面板
        if (HistorySearchPanelIsOpen)
        {
            HistorySearchPanelIsOpen = false;
        }

        //  添加查询历史
        var uiHistorySearch = HistorySearchs.FirstOrDefault(x => x.KeyWord.Equals(keyWord));
        if (uiHistorySearch == null)
        {
            HistorySearchs.Add(new HistorySearchModel { KeyWord = keyWord, Count = 1 });
        }
        //  缓存查询历史
        var searchCount = 1;
        var keyWordModel = appService.GetHistorySearchs().FirstOrDefault(x => x.KeyWord.Equals(keyWord));
        if (keyWordModel != null)
        {
            //  记录次数加一
            searchCount += keyWordModel.Count;
        }
        appService.HistorySearchAddOrUpdate(new HistorySearchModel { Count = searchCount, KeyWord = keyWord });

        //  Call出内容列表
        var searchView = container.Get<SearchViewModel>();
        searchView.KeyWord = keyWord;
        ActivateItem(searchView);
    }
    public void PopupMouseOverControls_MouseLeave(object sender, MouseEventArgs e)
    {
        historySearchPanelDispatcherTimer.Start();
    }
    public void PopupMouseOverControls_MouseEnter(object sender, MouseEventArgs e)
    {
        historySearchPanelDispatcherTimer.Stop();
        HistorySearchPanelIsOpen = true;
    }
    public void SearchGotFocus()
    {
        HistorySearchPanelIsOpen = true;
        historySearchPanelDispatcherTimer.Stop();
    }
    public void HistorySearchPanelClose()
    {
        HistorySearchPanelIsOpen = false;
    }
    public void HistorySearchRemove(HistorySearchModel model)
    {
        if (model != null)
        {
            if (!string.IsNullOrEmpty(model.KeyWord))
            {
                appService.HistorySearchTryRemove(model.KeyWord);
            }
            HistorySearchs.Remove(model);
        }

    }
    public void HistorySearchClearn()
    {
        //  先清除缓存
        foreach (var item in HistorySearchs)
        {
            if (!string.IsNullOrEmpty(item.KeyWord))
            {
                appService.HistorySearchTryRemove(item.KeyWord);
            }
        }
        //  UI更新
        HistorySearchs.Clear();
    }
    private Screen? GetScreen(string viewModel)
    {
        var type = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.Name.Equals(viewModel)).FirstOrDefault();
        if (type != null)
        {
            return (Screen)container.Get(type);
        }
        return default(Screen);
    }
}
