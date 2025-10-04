using RadianTools.UI.Avalonia.Common;
using System.Collections.ObjectModel;

namespace RadianTools.UI.Avalonia.ViewModels;

public class FolderTreeViewModel : ViewModelBase
{
    private readonly IFolderItemFactory _factory;

    public FolderTreeViewModel() : this(new MockFolderItemFactory())
    {
    }

    public FolderTreeViewModel(IFolderItemFactory factory)
    {
        _factory = factory;
        RootItems = new ObservableCollection<FolderTreeItemViewModel>(
            factory.GetRootItems().Select(
                item => new FolderTreeItemViewModel(factory, item)
            )
        );
    }

    public ObservableCollection<FolderTreeItemViewModel> RootItems { get; }

    private FolderTreeItemViewModel? _selectedItem;
    public FolderTreeItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    public bool ExpandFromTreePath(string treePath)
    {
        if (string.IsNullOrEmpty(treePath))
            return false;

        var parts = treePath.Split(Path.DirectorySeparatorChar);
        var currentItems = RootItems;
        FolderTreeItemViewModel? current = null;

        foreach (var part in parts)
        {
            var next = currentItems.FirstOrDefault(x => x.Name == part);
            if (next == null)
                break; // ここで終了 → 途中までしか見つからなかった

            current = next;
            current.IsExpanded = true;
            current.LoadChildren();
            currentItems = current.Children;
        }

        if (current != null)
        {
            SelectedItem = current;
            return true;
        }

        return false;
    }
}
