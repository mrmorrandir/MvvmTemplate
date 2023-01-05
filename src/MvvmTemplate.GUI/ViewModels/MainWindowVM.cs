using System.Windows;
using Microsoft.Extensions.Logging;
using MvvmTemplate.GUI.ViewModels.Infos;
using MvvmTemplate.GUI.ViewModels.Messages;
using WSE.Users.Controls;

namespace MvvmTemplate.GUI.ViewModels;

public sealed class MainWindowVM : ViewModelBase
{
    private readonly ISession _session;
    private readonly IMessagesVM _messagesVM;
    private readonly ILoginVM _loginVM;
    private readonly IReleaseNotesVM _releaseNotesVM;
    private readonly INotesVM _notesVM;
    private readonly ISettingsVM _settingsVM;
    private IViewModelBase? _current;
    
    public IMessagesVM Messages => _messagesVM;

    public IViewModelBase Current
    {
        get => _current;
        set => SetProperty(ref _current, value);
    }
    
    public DelegateCommand StartCommand { get; private set; }
    public DelegateCommand ExitCommand { get; private set; }
    public DelegateCommand LoginCommand { get; private set; }
    public DelegateCommand LogoutCommand { get; private set; }
    public DelegateCommand SettingsCommand { get; private set; }
    public DelegateCommand ReleaseNotesCommand { get; private set; }
    public DelegateCommand NotesCommand { get; private set; }
    
    
    public MainWindowVM(
        ILogger<MainWindowVM> logger,
        ISession session,
        IMessagesVM messagesVM,
        ILoginVM loginVM,
        IReleaseNotesVM releaseNotesVM,
        INotesVM notesVM,
        ISettingsVM settingsVM)
    {
        _session = session;
        _messagesVM = messagesVM;
        _loginVM = loginVM;
        _releaseNotesVM = releaseNotesVM;
        _notesVM = notesVM;
        _settingsVM = settingsVM;

        CreateCommandBinding();
        
        session.AuthenticatedUserChanged += (sender, args) => Current = args.AuthenticatedUser is not null ? _releaseNotesVM : _loginVM;
        Current = _loginVM;
    }

    private void Start(object obj)
    {
        // TODO: Let the magic happen!
    }
    
    private void Exit(object obj)
    {
        Application.Current.Shutdown();
    }
    
    private void Login(object obj)
    {
        Current = _loginVM;
    }
    
    private void Logout(object obj)
    {
        _session.Logout();
    }
    
    private void Settings(object obj)
    {
        Current = _settingsVM;
    }
    
    private void ReleaseNotes(object obj)
    {
        Current = _releaseNotesVM;
    }
    
    private void Notes(object obj)
    {
        _notesVM.NotesFile = "Help.md";
        Current = _notesVM;
    }
    
    
    
    
}