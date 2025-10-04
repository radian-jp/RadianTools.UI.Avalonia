namespace RadianTools.UI.Avalonia.Common;

public static class IFolderItemExtentions
{
    public static string MakeTreePath(this IFolderItem source, IFolderItem? parent)
        => parent != null ? Path.Combine(parent.TreePath, source.DisplayName) : source.DisplayName;
}
