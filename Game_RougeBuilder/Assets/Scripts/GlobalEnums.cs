using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// 游戏阶段类型
/// </summary>
public enum EGamePeriodType
{
    None,
    Fight, // 相互攻击
    SelectTalent, // 选择天赋
}

/// <summary>
/// 游戏阶段通过条件
/// </summary>
public enum EGamePeriodPassCondition
{
    Time, // 经过时间
    TalentPoolCount, // 检查天赋池数量
}

public enum EFightProperty
{
    None = 0,

    // ===== 1~100 是基础值 ===== 

    /// <summary>
    /// 当前血量
    /// </summary>
    hpCur = 1,

    /// <summary>
    /// 最大血量
    /// </summary>
    hpMax_Basic,

    /// <summary>
    /// 攻击力
    /// </summary>
    attack_Basic,

    /// <summary>
    /// 攻击间隔 (s)
    /// </summary>
    attackInterval_Basic,

    /// <summary>
    /// 攻击暴击率 (0~100), 计算时 randomVal <= rate 为暴击
    /// </summary>
    attackCritRate_Basic,

    /// <summary>
    /// 闪避率 (0~100), 计算时 randomVal <= rate 为闪避成功
    /// </summary>
    missRate_Basic,

    // ===== 超过100是修改值 ===== 

    hpMax_Modify = 101,
    attack_Modify,
    attackInterval_Modify,
    attackCritRate_Modify,
    missRate_Modify,

    // ===== 超过200是计算值 ===== 

    hpMax = 201,
    attack,
    attackInterval,
    attackCritRate,
    missRate,

}

public enum EBuffType
{
    FightProperty,
    Stun,
}