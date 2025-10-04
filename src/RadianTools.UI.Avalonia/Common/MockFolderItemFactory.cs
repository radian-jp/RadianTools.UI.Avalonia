using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianTools.UI.Avalonia.Common;

internal class MockFolderItemFactory : IFolderItemFactory
{
    public IFolderItem GetDummyItem()
    {
        return new MockFolderItem();
    }

    public IReadOnlyList<IFolderItem> GetRootItems()
    {
        return new List<IFolderItem>
        {
            new MockFolderItem("Please call [FolderItemFactoryProvider.Register] first!"),
            new MockFolderItem("Root A", new[]
            {
                new MockFolderItem("Child A1"),
                new MockFolderItem("Child A2", new[]
                {
                    new MockFolderItem("Grandchild A2-1")
                })
            }),
            new MockFolderItem("Root B")
        };
    }
}
