namespace RadianTools.UI.Avalonia.Common;

public interface IFolderItemFactory
{
    IReadOnlyList<IFolderItem> GetRootItems();
    IFolderItem GetDummyItem();
}