namespace RadianTools.UI.Avalonia.Common;

public static class FolderItemFactoryProvider
{
    private static IFolderItemFactory? _factory;

    public static void Register(IFolderItemFactory factory)
    {
        _factory = factory;
    }

    public static IFolderItemFactory Create()
    {
        if (_factory == null)
            return new MockFolderItemFactory();

        return _factory;
    }
}
