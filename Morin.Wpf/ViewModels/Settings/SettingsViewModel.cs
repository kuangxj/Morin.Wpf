using Morin.Services;
using Morin.Shared.Models;
using Stylet;
using StyletIoC;
using System.Reflection;

namespace Morin.Wpf.ViewModels.Settings;

public class SettingsViewModel(IContainer container, IAppService appService) : Conductor<Screen>
{
    private MenuModel menuItem;
    private readonly IContainer container = container;
    private readonly IAppService appService = appService;

    public MenuModel MenuItem
    {
        get { return menuItem; }
        set
        {
            menuItem = value;
            Navigate(value);
        }
    }

    public BindableCollection<MenuModel> Menus { get; set; }

    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();
        var menus = appService.GetMenus();
        var menu = menus.FirstOrDefault(x => x.Title != null && x.Title.Equals("设置"));
        if (menu != null)
        {
            Menus = [.. menus.Where(x => x.Pid == menu.Id && x.Visvisibility == true)];
        }
    }


    private void Navigate(MenuModel menuBar)
    {
        var x = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.Name.Equals(menuBar.ViewModel)).FirstOrDefault();
        if (x != null)
        {
            ActivateItem((Screen)container.Get(x));
        }
    }
}
