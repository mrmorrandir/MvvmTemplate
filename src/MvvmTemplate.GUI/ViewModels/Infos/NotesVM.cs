namespace MvvmTemplate.GUI.ViewModels.Infos;

public class NotesVM : ViewModelBase, INotesVM
{
    private string _notesFile;
    public string NotesFile
    {
        get => _notesFile;
        set => SetProperty(ref _notesFile, value);
    }

    public NotesVM()
    {
        
    }
}