using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using RadianTools.UI.Avalonia.Common;
using RadianTools.UI.Avalonia.ViewModels;

namespace RadianTools.UI.Avalonia.Controls;

public partial class FolderTreeView : UserControl, IDisposable
{
    // SelectedItem (読み取り専用, バインディング用)
    public static readonly StyledProperty<FolderTreeItemViewModel?> SelectedItemProperty =
        AvaloniaProperty.Register<FolderTreeView, FolderTreeItemViewModel?>(
            nameof(SelectedItem), defaultBindingMode: BindingMode.OneWayToSource);

    public FolderTreeItemViewModel? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        private set => SetValue(SelectedItemProperty, value);
    }

    // SelectedTreePath (TwoWay)
    public static readonly StyledProperty<string?> SelectedTreePathProperty =
        AvaloniaProperty.Register<FolderTreeView, string?>(
            nameof(SelectedTreePath), defaultBindingMode: BindingMode.TwoWay);

    public string? SelectedTreePath
    {
        get => GetValue(SelectedTreePathProperty);
        set => SetValue(SelectedTreePathProperty, value);
    }

    // RoutedEvent: FolderSelected
    public static readonly RoutedEvent<FolderSelectedEventArgs> FolderSelectedEvent =
        RoutedEvent.Register<FolderTreeView, FolderSelectedEventArgs>(
            nameof(FolderSelected), RoutingStrategies.Bubble);

    public event EventHandler<FolderSelectedEventArgs> FolderSelected
    {
        add => AddHandler(FolderSelectedEvent, value);
        remove => RemoveHandler(FolderSelectedEvent, value);
    }

    private readonly FolderTreeViewModel _vm;
    private readonly TreeView _treeView;
    private bool _disposed;

    public FolderTreeView()
    {
        InitializeComponent();

        _vm = new FolderTreeViewModel(FolderItemFactoryProvider.Create());
        _treeView = this.FindControl<TreeView>("treeView")!;
        _treeView.DataContext = _vm;

        _vm.PropertyChanged += OnVmPropertyChanged;

        // Control.SelectedTreePath → VM 同期
        this.GetObservable(SelectedTreePathProperty).Subscribe(path =>
        {
            if (string.IsNullOrEmpty(path))
                return;

            if (!_vm.ExpandFromTreePath(path))
                return;

            var container = FindTreeViewItem(_vm.SelectedItem);
            container?.BringIntoView();
        });
    }

    private void OnVmPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(FolderTreeViewModel.SelectedItem))
            return;

        SelectedItem = _vm.SelectedItem;
        SelectedTreePath = _vm.SelectedItem?.Item.TreePath;

        if (SelectedItem == null)
            return;

        RaiseEvent(new FolderSelectedEventArgs(
            FolderSelectedEvent,
            SelectedItem.Item
        ));
    }

    private TreeViewItem? FindTreeViewItem(object? item)
    {
        if (_treeView == null || item == null)
            return null;

        return _treeView.GetVisualDescendants()
            .OfType<TreeViewItem>()
            .FirstOrDefault(tvi => tvi.DataContext == item);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        Dispose();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        foreach (var item in _vm.RootItems)
            DisposeRecursive(item);

        _vm.PropertyChanged -= OnVmPropertyChanged;
    }

    private void DisposeRecursive(FolderTreeItemViewModel vmItem)
    {
        vmItem.Item.Dispose();
        foreach (var child in vmItem.Children)
            DisposeRecursive(child);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}

// RoutedEventArgs
public class FolderSelectedEventArgs : RoutedEventArgs
{
    public IFolderItem Item { get; }

    public FolderSelectedEventArgs(RoutedEvent routedEvent, IFolderItem item)
        : base(routedEvent)
    {
        Item = item;
    }
}
