using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianTools.UI.Avalonia.Common;

internal class MockFolderItem : IFolderItem
{
    public IFolderItem? Parent { get; }

    public string TreePath => "";

    public string FilePath => "";

    public string DisplayName { get; }

    public bool IsFolder => true;

    public bool HasSubFolder { get; }

    public object? Icon => null;

    public bool IsDummy { get; }

    private IEnumerable<IFolderItem> _childs = [];

    public MockFolderItem(string name, IEnumerable<IFolderItem>? childs = null)
    {
        DisplayName = name;
        HasSubFolder = childs != null;
        IsDummy = false;
        if(childs != null )
        {
            _childs = childs.ToList();
        }
    }

    public MockFolderItem()
    {
        DisplayName = "";
        IsDummy = true;
    }

    public void Dispose()
    {
    }

    public IEnumerable<IFolderItem> EnumAllChilds()
    {
        return _childs;
    }

    public IEnumerable<IFolderItem> EnumFiles()
    {
        return Array.Empty<IFolderItem>();
    }

    public IEnumerable<IFolderItem> EnumFolders()
    {
        return _childs;
    }
}
