using System.Collections.ObjectModel;
using MLib3.Communication.MessageBroker.Abstractions;

namespace MvvmTemplate.GUI.ViewModels.Messages;

public interface IMessagesVM : IViewModelBase
{
    ObservableCollection<IMessage> Messages { get; }
    public bool HasMessages { get; }
    public bool IsResettable { get; }
    public DelegateCommand ResetCommand { get; }
}