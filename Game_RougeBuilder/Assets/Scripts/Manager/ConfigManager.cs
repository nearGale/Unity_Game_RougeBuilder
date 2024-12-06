using System.Collections.Generic;

public static class ConfigManager
{
    public static List<IManager> managers = new List<IManager>
    {
        ManagerConfig.Create(),
        ManagerGamePeriod.Create(),
        ManagerBigMap.Create(),
        ManagerMonster.Create(),
        ManagerTalentPool.Create(),
    };
}