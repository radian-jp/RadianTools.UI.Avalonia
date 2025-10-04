using RadianTools.UI.Avalonia.Common;
using System.Collections.ObjectModel;

namespace RadianTools.UI.Avalonia.ViewModels;

public class FolderTreeItemViewModel : ViewModelBase
{
    public IFolderItem Item { get; }
    public IFolderItemFactory ItemFactory { get; }
    public string Name => Item.DisplayName;
    public string FilePath => Item.FilePath;
    public string TreePath => Item.TreePath;
    public object? Icon => Item.Icon;

    public ObservableCollection<FolderTreeItemViewModel> Children { get; } = new();

    private bool _isExpanded;
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (SetProperty(ref _isExpanded, value) && _isExpanded)
                LoadChildren();
        }
    }

    private bool _isLoaded;

    public FolderTreeItemViewModel(IFolderItemFactory itemFactory, IFolderItem item)
    {
        ItemFactory = itemFactory;
        Item = item;
        if (Item.HasSubFolder)
            Children.Add(new FolderTreeItemViewModel(itemFactory, itemFactory.GetDummyItem()));
    }

    public void LoadChildren()
    {
        if (_isLoaded)
            return;

        _isLoaded = true;

        if (Children.Count == 1 && Children[0].Item.IsDummy)
            Children.Clear();

        try
        {
            var folders = Item.GetFolders();
            foreach (var folder in folders)
                Children.Add(new FolderTreeItemViewModel(ItemFactory, folder));
        }
        catch
        {
        }
    }
}
