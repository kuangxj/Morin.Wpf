#if DEBUG
using System.Diagnostics;
#endif
using AutoMapper;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Morin.Services;
using Morin.Shared.Configs;
using Morin.Storages;
using Morin.Wpf.Profiles;
using Morin.Wpf.ViewModels;
using Stylet;
using StyletIoC;
using System.Windows.Threading;
using Morin.Wpf.Adapters;

namespace Morin.Wpf;

public class Bootstrapper : Bootstrapper<ShellViewModel>
{

    protected override void OnStart()
    {
        base.OnStart();

#if DEBUG
        Stylet.Logging.LogManager.Enabled = true;
#endif


    }


    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        base.ConfigureIoC(builder);

        //  配置
        var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        builder.Bind<IConfiguration>().ToFactory(x => configBuilder).InSingletonScope();

        //  应用配置注入
        builder.Bind<AppSettingsConfig>().ToFactory(o => configBuilder.GetSection("AppSettings").Get<AppSettingsConfig>());

        //  映射配置注入
        builder.Bind<IMapper>().ToFactory(x => new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapperProfile());
        }).CreateMapper()).InSingletonScope();
        builder.Bind<ISnackbarMessageQueue>().To<SnackbarMessageQueue>().InSingletonScope();

        //  服务注入
        builder.Bind<IApiService>().To<ApiService>().InSingletonScope();
        builder.Bind<IAppService>().To<AppService>().InSingletonScope();
        //  存储注入
        builder.Bind<IAppStorage>().To<AppStorage>().InSingletonScope();

        //  来源协议适配器注入
        builder.Bind<ISourceProtocolAdapter>().To<ThinkPhpSourceProtocolAdapter>().InSingletonScope();

    }


    protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
    {
        base.OnUnhandledException(e);

#if DEBUG
        Debug.WriteLine(e.Exception.ToString());
#endif
    }

}
