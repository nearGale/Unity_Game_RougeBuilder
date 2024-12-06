using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalParams
{
    /// <summary>
    /// monster 攻击暴击伤害倍率
    /// </summary>
    public const float MonsterAttackCritDmgMultiplier = 2f;

    /// <summary>
    /// 大地图配置相对路径（Asset下）
    /// </summary>
    public const string ConfigPathBigMap = @"/Excels/Map/BigMap.xlsx";

    /// <summary>
    /// 大地图表中标识符
    /// </summary>
    public const string BigMapGridStaticDataWall = "W";

    public const string BigMapGridStaticDataMonster = "M";

    public const string BigMapGridStaticDataNPC = "N";
}
