using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using MLib3.Communication.MessageBroker.Abstractions;
using MLib3.Communication.MessageBroker.Messages;

namespace MvvmTemplate.GUI.ViewModels.Messages;

public class MessagesVM : ViewModelBase, IMessagesVM
{
    public ObservableCollection<IMessage> Messages { get; } = new();
    public bool HasMessages => Messages.Any();
    public bool IsResettable => AnyMessageResettable();

    public DelegateCommand ResetCommand { get; }

    public MessagesVM(IMessageBroker messageBroker)
    {
        messageBroker.Subscribe<ErrorMessage>(OnErrorMessage);
        messageBroker.Subscribe<ErrorResolvedMessage>(OnErrorResolvedMessage);
        messageBroker.Subscribe<TempInfoMessage>(OnTempInfoMessage);
        messageBroker.Subscribe<InfoMessage>(OnInfoMessage);

        ResetCommand = new DelegateCommand(Reset, o => IsResettable);
        ResetCommand.ReactsTo(this, nameof(Messages)).ReactsTo(this, nameof(IsResettable)).Activate();
        Messages.CollectionChanged += (sender, args) =>
        {
            RaisePropertyChanged(nameof(HasMessages));
            RaisePropertyChanged(nameof(IsResettable));
        };

        var tempMessageTimer = new Timer(250);
        tempMessageTimer.Elapsed += TempMessageTimerElapsed;
        tempMessageTimer.Start();
    }

    private void OnInfoMessage(InfoMessage infoMessage)
    {
        Application.Current.Dispatcher.Invoke(() => Messages.Add(infoMessage));
    }

    private void TempMessageTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        foreach (var message in Messages.ToArray())
            if (message is TempInfoMessage tempInfoMessage)
                if (DateTime.Now >= tempInfoMessage.Timestamp.Add(tempInfoMessage.Timeout))
                    Application.Current.Dispatcher.Invoke(() => Messages.Remove(tempInfoMessage));
    }

    private void OnTempInfoMessage(TempInfoMessage tempInfoMessage)
    {
        Application.Current.Dispatcher.Invoke(() => Messages.Add(tempInfoMessage));
    }

    private bool AnyMessageResettable()
    {
        return Application.Current.Dispatcher.Invoke(() => Messages.Any(m => m is ErrorMessage { IsResettable: true } or InfoMessage));
    }

    private void Reset(object obj)
    {
        foreach (var message in Messages.ToArray())
        {
            switch (message)
            {
                case InfoMessage infoMessage:
                    Messages.Remove(infoMessage);
                    break;
                case ErrorMessage { IsResettable: true } errorMessage:
                    errorMessage.Reset();
                    break;
            }
        }
    }

    private void OnErrorResolvedMessage(ErrorResolvedMessage errorResolvedMessage)
    {
        var message = Messages.SingleOrDefault(m => m.Id == errorResolvedMessage.ErrorId);
        if (message is not null)
            Application.Current.Dispatcher.Invoke(() => Messages.Remove(message));
    }

    private void OnErrorMessage(ErrorMessage errorMessage)
    {
        Application.Current.Dispatcher.Invoke(() => Messages.Add(errorMessage));
    }
}