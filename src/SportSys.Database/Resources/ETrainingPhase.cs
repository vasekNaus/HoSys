using System.Resources;

namespace SportSys.Database.Resources;

public static class ETrainingPhase
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "SportSys.Database.Resources.ETrainingPhase",
            typeof(ETrainingPhase).Assembly);
}
