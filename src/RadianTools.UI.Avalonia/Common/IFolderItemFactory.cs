namespace RadianTools.UI.Avalonia.Common;

public enum FolderRootMode
{
    DesktopVirtualFolders,
    LogicalDrives
}

public interface IFolderItemFactory
{
    FolderRootMode RootMode { get; set; }
    IEnumerable<IFolderItem> EnumRootItems();
    IFolderItem GetDummyItem();
}
