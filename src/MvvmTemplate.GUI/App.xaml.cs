using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MvvmTemplate.GUI.ViewModels;
using NLog.Extensions.Logging;

namespace MvvmTemplate.GUI;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    private async void App_OnStartup(object sender, StartupEventArgs e)
    {
        _host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddGUI();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.AddNLog("nlog.config");
            })
            .Build();

        await _host.StartAsync();

        _host.Services.UseUserAdministrationSession(Current);
        _host.Services.UseGUI();

        var mainWindowVM = _host.Services.GetRequiredService<MainWindowVM>();
        var mainWindow = new MainWindow
        {
            DataContext = mainWindowVM
        };
        mainWindow.Show();
    }

    private async void App_OnExit(object sender, ExitEventArgs e)
    {
        using (_host)
        {
            await _host!.StopAsync();
        }
    }
}