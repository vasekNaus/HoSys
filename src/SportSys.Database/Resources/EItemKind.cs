using System.Resources;

namespace SportSys.Database.Resources;

public static class EItemKind
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "SportSys.Database.Resources.EItemKind",
            typeof(EItemKind).Assembly);
}
