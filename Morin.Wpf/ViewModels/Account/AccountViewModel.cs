using Morin.Services;
using Morin.Shared.Models;
using Stylet;
using StyletIoC;
using System.Reflection;

namespace Morin.Wpf.ViewModels.Account;

internal class AccountViewModel(IContainer container, IAppService appService) : Conductor<Screen>
{
    private readonly IContainer? container = container;
    private readonly IAppService? appService = appService;
    private MenuModel? menuItem;
    public MenuModel? MenuItem
    {
        get => menuItem; set
        {
            menuItem = value;
            if (value != null && !string.IsNullOrEmpty(value.ViewModel))
            {
                ActivateItem(GetScreen(value.ViewModel));
            }
        }
    }
    public BindableCollection<MenuModel> Menus { get; set; } = [];

    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();
        SetMenusAsync("我的");

        //  加载历史观看
        appService?.LoadHistoryViews();

        //  加载收藏
        appService?.LoadFavorites();
    }
    private void SetDefaultMenu()
    {
        MenuItem = Menus.OrderBy(x => x.Sort).First();
    }
    private Task SetMenusAsync(string rootTitle)
    {
        return Task.Run(() =>
        {
            var menus = appService?.GetMenus();
            var rootMenu = menus?.FirstOrDefault(x => x.Title != null && x.Title.Contains(rootTitle));
            if (rootMenu != null)
            {
                var nodeMenus = menus?.Where(x => x.Pid == rootMenu.Id);
                Menus = [.. nodeMenus];

                SetDefaultMenu();
            }
        });
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
