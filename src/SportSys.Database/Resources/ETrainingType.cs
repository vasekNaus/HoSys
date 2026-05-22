using System.Resources;

namespace SportSys.Database.Resources;

public static class ETrainingType
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "SportSys.Database.Resources.ETrainingType",
            typeof(ETrainingType).Assembly);
}
