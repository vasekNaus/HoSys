using System.Resources;

namespace SportSys.Database.Resources;

public static class EMatchType
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "SportSys.Database.Resources.EMatchType",
            typeof(EMatchType).Assembly);
}
