using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum EFightProperty
{
    // ===== 0~100 是基础值 ===== 

    /// <summary>
    /// 当前血量
    /// </summary>
    hpCur = 1,

    /// <summary>
    /// 最大血量
    /// </summary>
    hpMax_Basic = 2,

    /// <summary>
    /// 攻击力
    /// </summary>
    attack_Basic = 3,

    /// <summary>
    /// 攻击间隔 (s)
    /// </summary>
    attackInterval_Basic = 4,

    /// <summary>
    /// 攻击暴击率 (0~100), 计算时 randomVal <= rate 为暴击
    /// </summary>
    attackCritRate_Basic = 5,

    /// <summary>
    /// 闪避率 (0~100), 计算时 randomVal <= rate 为闪避成功
    /// </summary>
    missRate_Basic = 6,

    // ===== 超过100是修改值 ===== 

    hpMax_Modify = 101,
    attack_Modify = 102,
    attackInterval_Modify = 103,
    attackCritRate_Modify = 104,
    missRate_Modify = 105,

    // ===== 超过200是计算值 ===== 

    hpMax = 201,
    attack = 202,
    attackInterval = 203,
    attackCritRate = 204,
    missRate = 205,

}

public class Monster
{
    #region [Property]
    public Dictionary<EFightProperty, float> fightProperties = new();

    public int id;

    public bool alive => GetFightProperty(EFightProperty.hpCur) > 0;

    private float _lastAttackTime;

    public int targetMonsterId;

    public UIMonsterCtrl ui;

    public List<int> talents = new();

    #endregion [Property]

    #region [Life Circle Method]

    public void Reset()
    {
        foreach(var kvPair in fightProperties)
        {
            // 清除中途修改值
            if((int)kvPair.Key > 100 && (int)kvPair.Key <= 200)
            {
                fightProperties[kvPair.Key] = 0;
            }
        }

        SetFightProperty(EFightProperty.hpCur, GetFightProperty(EFightProperty.hpMax));

        talents.Clear();
        ui.RefreshTalents(talents);
    }

    #endregion [Life Circle Method]

    #region [Fight Property Method]

    public float GetFightProperty(EFightProperty prop)
    {
        var val = 0f;

        switch (prop)
        {
            case EFightProperty.hpMax:
                val = GetFightProperty(EFightProperty.hpMax_Basic) + GetFightProperty(EFightProperty.hpMax_Modify);
                break;
            case EFightProperty.attack:
                val = GetFightProperty(EFightProperty.attack_Basic) + GetFightProperty(EFightProperty.attack_Modify);
                break;
            case EFightProperty.attackInterval:
                val = GetFightProperty(EFightProperty.attackInterval_Basic) + GetFightProperty(EFightProperty.attackInterval_Modify);
                break;
            case EFightProperty.attackCritRate:
                val = GetFightProperty(EFightProperty.attackCritRate_Basic) + GetFightProperty(EFightProperty.attackCritRate_Modify);
                break;
            case EFightProperty.missRate:
                val = GetFightProperty(EFightProperty.missRate_Basic) + GetFightProperty(EFightProperty.missRate_Modify);
                break;
            default:
                if (fightProperties.TryGetValue(prop, out var value))
                {
                    val = value;
                }
                break;
        }

        var clampedVal = ClampFightProperty(prop, val);

        return clampedVal;
    }

    public void SetFightProperty(EFightProperty prop, float val)
    {
        fightProperties[prop] = val;

        OnPropertyChanged(prop);
    }

    private float ClampFightProperty(EFightProperty prop, float val)
    {
        switch (prop)
        {
            case EFightProperty.hpMax:
                val = Mathf.Max(1, val);
                break;
            case EFightProperty.hpCur:
                val = Mathf.Max(0, val);
                break;
            case EFightProperty.attack:
                val = Mathf.Max(1, val);
                break;
            case EFightProperty.attackInterval:
                val = Mathf.Max(0.05f, val);
                break;
            case EFightProperty.attackCritRate:
                val = Mathf.Max(0, val);
                break;
            case EFightProperty.missRate:
                val = Mathf.Max(0, val);
                break;
            default:
                break;
        }
        return val;
    }

    private void OnPropertyChanged(EFightProperty prop)
    {
        switch (prop)
        {
            case EFightProperty.hpMax_Basic:
            case EFightProperty.hpMax_Modify:
            case EFightProperty.hpCur:
                ui.RefreshHp(
                    (int)GetFightProperty(EFightProperty.hpCur),
                    (int)GetFightProperty(EFightProperty.hpMax));
                break;
            case EFightProperty.attack_Basic:
            case EFightProperty.attack_Modify:
                ui.RefreshAttack((int)GetFightProperty(EFightProperty.attack));
                break;
            case EFightProperty.attackInterval_Basic:
            case EFightProperty.attackInterval_Modify:
                ui.RefreshAttackInterval(GetFightProperty(EFightProperty.attackInterval));
                break;
            case EFightProperty.attackCritRate_Basic:
            case EFightProperty.attackCritRate_Modify:
                ui.RefreshAttackCritRate((int)GetFightProperty(EFightProperty.attackCritRate));
                break;
            case EFightProperty.missRate_Basic:
            case EFightProperty.missRate_Modify:
                ui.RefreshAttackedMissingRate((int)GetFightProperty(EFightProperty.missRate));
                break;
            default:
                break;
        }
    }

    public void SetFightProperty(int hpMax, int attack, float attackInterval,
        int attackCritRate, int missRate)
    {
        SetFightProperty(EFightProperty.attack_Basic, attack);
        SetFightProperty(EFightProperty.hpMax_Basic, hpMax);
        SetFightProperty(EFightProperty.attackInterval_Basic, attackInterval);
        SetFightProperty(EFightProperty.attackCritRate_Basic, attackCritRate);
        SetFightProperty(EFightProperty.missRate_Basic, missRate);
    }

    #endregion [Fight Property Method]

    public void SetId(int id)
    {
        this.id = id;

        ui.RefreshName($"Monster_{id}");
    }

    public void TryAttack()
    {
        if (!alive) return;

        if (_lastAttackTime == 0 || Time.time > _lastAttackTime + GetFightProperty(EFightProperty.attackInterval))
        {
            _lastAttackTime = Time.time;

            DoAttack();
        }
    }

    private void DoAttack()
    {
        if (targetMonsterId == 0) return;

        var targetMonster = ManagerMonster.Instance.GetMonsterById(targetMonsterId);
        if (targetMonster == null) return;
        if (!targetMonster.alive) return;

        // 计算闪避
        bool isMissing = false;
        int randomVal = Random.Range(0, 100);
        if (randomVal < targetMonster.GetFightProperty(EFightProperty.missRate))
        {
            isMissing = true;
            targetMonster.OnAttackedMissing();
            return;
        }

        // 计算暴击
        bool isCritical = false;
        randomVal = Random.Range(0, 100);
        if(randomVal <= GetFightProperty(EFightProperty.attackCritRate))
        {
            isCritical = true;
        }

        var dmgVal = (int)GetFightProperty(EFightProperty.attack);

        if (isCritical)
        {
            dmgVal = (int)(dmgVal * GlobalParams.MonsterAttackCritDmgMultiplier);
        }

        targetMonster.OnAttacked(dmgVal, isCritical);
    }

    /// <summary>
    /// 当自己受到攻击时
    /// </summary>
    /// <param name="dmgVal">伤害数值</param>
    /// <param name="isCritical">是否是暴击</param>
    public void OnAttacked(int dmgVal, bool isCritical)
    {
        var curHp = Mathf.Max(0, GetFightProperty(EFightProperty.hpCur) - dmgVal);
        SetFightProperty(EFightProperty.hpCur, curHp);

        if (curHp == 0)
        {
            OnDead();
        }

        ui.ShowDmgTaken(dmgVal, isCritical);
    }

    /// <summary>
    /// 当自己闪避了敌方攻击时
    /// </summary>
    public void OnAttackedMissing()
    {
        ui.ShowAttackedMissing();
    }

    public void AddTalent(int id)
    {
        talents.Add(id);
        ui.RefreshTalents(talents);

        // 效果生效
        var randomVal = Random.Range(102, 106);
        float randomVal2 = Random.Range(0, 10);

        if (randomVal == 103)
        {
            randomVal2 = Random.Range(-0.2f, 0.2f);
        }

        SetFightProperty(
            (EFightProperty)randomVal,
            GetFightProperty((EFightProperty)randomVal) + randomVal2
            );
    }

    private void OnDead() { }



}
