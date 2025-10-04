using RadianTools.UI.Avalonia.Common;

namespace RadianTools.UI.Avalonia.Windows;

public enum WindowsFolderRootMode
{
    DesktopVirtualFolders,
    LogicalDrives
}

public class WindowsFolderItemFactory : IFolderItemFactory
{
    private readonly WindowsFolderRootMode _mode;

    private static readonly Guid[] _IgnoreItems = {
        FOLDERID.UsersLibraries,
        FOLDERID.UsersFiles,
        FOLDERID.NetworkFolder,
    };

    public WindowsFolderItemFactory(WindowsFolderRootMode mode = WindowsFolderRootMode.DesktopVirtualFolders)
    {
        _mode = mode;
    }

    public IReadOnlyList<IFolderItem> GetRootItems()
    {
        return _mode switch
        {
            WindowsFolderRootMode.DesktopVirtualFolders => GetDesktopChildren(),
            WindowsFolderRootMode.LogicalDrives => GetLogicalDrives(),
            _ => new List<IFolderItem>()
        };
    }

    public IFolderItem GetDummyItem()
        => new WindowsFolderItem();

    private IReadOnlyList<IFolderItem> GetDesktopChildren()
    {
        var list = new List<IFolderItem>();
        var pidls = KnownFolderPIDL.Desktop.Value.GetFolders();
        foreach (var child in pidls.Where(x => x.IsKnownFolder && !_IgnoreItems.Contains(x.KnownFolderId)))
        {
            list.Add(new WindowsFolderItem(null, child));
        }
        return list;
    }

    private IReadOnlyList<IFolderItem> GetLogicalDrives()
    {
        return DriveInfo.GetDrives()
            .Where(d => d.DriveType == DriveType.Fixed || d.DriveType == DriveType.Removable)
            .Select(d => SafePIDL.FromFilePath(d.Name))
            .Select(pidl => new WindowsFolderItem(null, pidl))
            .ToList();
    }
}
