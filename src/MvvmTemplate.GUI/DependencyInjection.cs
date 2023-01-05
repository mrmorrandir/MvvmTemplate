using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MLib3.Communication.MessageBroker;
using MvvmTemplate.GUI.ViewModels;
using MvvmTemplate.GUI.ViewModels.Infos;
using MvvmTemplate.GUI.ViewModels.Messages;

namespace MvvmTemplate.GUI;

public static class DependencyInjection
{
    public static IServiceCollection AddGUI(this IServiceCollection services)
    {
        services.AddUserAdministration(config => config.BaseAddress = new Uri("https://api-ab.wittag.net/UsersAPI/"));
        services.AddMessageBroker();
        
        services.AddSingleton<MainWindowVM>();
        services.AddSingleton<IMessagesVM, MessagesVM>();
        services.AddSingleton<IReleaseNotesVM,ReleaseNotesVM>();
        services.AddSingleton<INotesVM,NotesVM>();
        services.AddSingleton<ISettingsVM,SettingsVM>();
     
        return services;
    }
    
    public static IServiceProvider UseGUI(this IServiceProvider serviceProvider)
    {
        serviceProvider.UseUserAdministrationSession(Application.Current);
        return serviceProvider;
    }
}