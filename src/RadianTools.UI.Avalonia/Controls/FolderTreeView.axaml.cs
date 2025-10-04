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
    public static readonly StyledProperty<IFolderItem?> SelectedItemProperty =
        AvaloniaProperty.Register<FolderTreeView, IFolderItem?>(
            nameof(SelectedItem), defaultBindingMode: BindingMode.OneWayToSource);

    public IFolderItem? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        private set => SetValue(SelectedItemProperty, value);
    }

    public static readonly StyledProperty<string?> SelectedTreePathProperty =
        AvaloniaProperty.Register<FolderTreeView, string?>(
            nameof(SelectedTreePath), defaultBindingMode: BindingMode.TwoWay);

    public string? SelectedTreePath
    {
        get => GetValue(SelectedTreePathProperty);
        set => SetValue(SelectedTreePathProperty, value);
    }

    public static readonly RoutedEvent<SelectedItemChangedEventArgs> SelectedItemChangedEvent =
        RoutedEvent.Register<FolderTreeView, SelectedItemChangedEventArgs>(
            nameof(SelectedItemChanged), RoutingStrategies.Bubble);

    public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged
    {
        add => AddHandler(SelectedItemChangedEvent, value);
        remove => RemoveHandler(SelectedItemChangedEvent, value);
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

        // Control.SelectedTreePath ¨ VM “¯Šú
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

        SelectedItem = _vm.SelectedItem?.Item;
        SelectedTreePath = _vm.SelectedItem?.Item.TreePath;

        if (SelectedItem == null)
            return;

        RaiseEvent(new SelectedItemChangedEventArgs(
            SelectedItemChangedEvent,
            SelectedItem
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

public class SelectedItemChangedEventArgs : RoutedEventArgs
{
    public IFolderItem Item { get; }

    public SelectedItemChangedEventArgs(RoutedEvent routedEvent, IFolderItem item) : base(routedEvent)
    {
        Item = item;
    }
}
