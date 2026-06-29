using System.Resources;

namespace SportSys.Database.Resources;

public static class EItemStatus
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "SportSys.Database.Resources.EItemStatus",
            typeof(EItemStatus).Assembly);
}
