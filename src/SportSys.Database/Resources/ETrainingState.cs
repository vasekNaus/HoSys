using System.Resources;

namespace SportSys.Database.Resources;

public static class ETrainingState
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "SportSys.Database.Resources.ETrainingState",
            typeof(ETrainingState).Assembly);
}
