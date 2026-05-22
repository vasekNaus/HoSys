using System.Resources;

namespace SportSys.Database.Resources;

public static class ECoachRole
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "SportSys.Database.Resources.ECoachRole",
            typeof(ECoachRole).Assembly);
}
