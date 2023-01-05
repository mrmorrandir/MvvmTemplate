namespace MvvmTemplate.GUI.ViewModels;

public sealed class SettingsVM : ViewModelBase, ISettingsVM
{
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    
    public SettingsVM()
    {
        CreateCommandBinding();
    }

    public void Save(object obj)
    {
        
    }

    public void Cancel(object obj)
    {
        
    }
}