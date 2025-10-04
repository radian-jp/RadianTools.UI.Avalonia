namespace RadianTools.UI.Avalonia.Sample.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private string _selectedTreePath = "";
    public string SelectedTreePath
    {
        get => _selectedTreePath;
        set => SetProperty(ref _selectedTreePath, value);
    }
}
