using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Monster
{
    #region [Property]
    public Dictionary<EFightProperty, float> fightProperties = new();

    public int id;

    public int dataId;

    public bool alive => GetFightProperty(EFightProperty.curHp) > 0;

    private float _lastAttackTime;

    public int targetMonsterId;

    public UIMonsterCtrl ui;

    public List<int> talents = new();

    public ComponentBuff compBuff = new();

    private List<EFightProperty> _tmpList = new();

    #endregion [Property]

    #region [Life Circle Method]

    public void Reset()
    {
        _tmpList.Clear();
        foreach (var kvPair in fightProperties)
        {
            // 清除中途修改值
            if((int)kvPair.Key > 100 && (int)kvPair.Key <= 200)
            {
                _tmpList.Add(kvPair.Key);
            }
        }

        foreach (var key in _tmpList)
        {
            fightProperties[key] = 0;
        }
        _tmpList.Clear();

        fightProperties[EFightProperty.curHp] = GetFightProperty(EFightProperty.basicMaxHp);

        talents.Clear();
        ui.RefreshTalents(talents);

        compBuff.Clear();

        id = 0;
        dataId = 0;
    }

    public void BuffLogicUpdate()
    {
        compBuff.LogicUpdate();

        var stun = compBuff.HasBuffByType(EBuffType.Stun, out var _);
        ui.RefreshStun(stun);
    }

    #endregion [Life Circle Method]

    #region [Fight Property Method]

    public float GetFightProperty(EFightProperty prop)
    {
        var val = 0f;

        switch (prop)
        {
            case EFightProperty.calMaxHp:
                val = GetFightProperty(EFightProperty.basicMaxHp) + GetFightProperty(EFightProperty.modifyMaxHp);
                break;
            case EFightProperty.calAttack:
                val = GetFightProperty(EFightProperty.basicAttack) + GetFightProperty(EFightProperty.modifyAttack);
                break;
            case EFightProperty.calAttackInterval:
                val = GetFightProperty(EFightProperty.basicAttackInterval) + GetFightProperty(EFightProperty.modifyAttackInterval);
                break;
            case EFightProperty.calAttackCritRate:
                val = GetFightProperty(EFightProperty.basicAttackCritRate) + GetFightProperty(EFightProperty.modifyAttackCritRate);
                break;
            case EFightProperty.calMissRate:
                val = GetFightProperty(EFightProperty.basicMissRate) + GetFightProperty(EFightProperty.modifyMissRate);
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

    public void SetBasicProperty(MonsterExcelItem dataMonster)
    {
        dataId = dataMonster.id;

        ui.RefreshName(dataMonster.name);

        SetFightProperty(EFightProperty.basicMaxHp, dataMonster.basicHp);
        SetFightProperty(EFightProperty.curHp, dataMonster.basicHp);
        SetFightProperty(EFightProperty.basicAttack, dataMonster.basicAttack);
        SetFightProperty(EFightProperty.basicAttackInterval, dataMonster.attackInterval);
        SetFightProperty(EFightProperty.basicAttackCritRate, dataMonster.attackCritRate);
        SetFightProperty(EFightProperty.basicMissRate, dataMonster.missRate);

    }

    private float ClampFightProperty(EFightProperty prop, float val)
    {
        switch (prop)
        {
            case EFightProperty.calMaxHp:
                val = Mathf.Max(1, val);
                break;
            case EFightProperty.curHp:
                val = Mathf.Max(0, val);
                break;
            case EFightProperty.calAttack:
                val = Mathf.Max(1, val);
                break;
            case EFightProperty.calAttackInterval:
                val = Mathf.Max(0.05f, val);
                break;
            case EFightProperty.calAttackCritRate:
                val = Mathf.Max(0, val);
                break;
            case EFightProperty.calMissRate:
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
            case EFightProperty.basicMaxHp:
            case EFightProperty.modifyMaxHp:
            case EFightProperty.curHp:
                ui.RefreshHp(
                    (int)GetFightProperty(EFightProperty.curHp),
                    (int)GetFightProperty(EFightProperty.calMaxHp));
                break;
            case EFightProperty.basicAttack:
            case EFightProperty.modifyAttack:
                ui.RefreshAttack((int)GetFightProperty(EFightProperty.calAttack));
                break;
            case EFightProperty.basicAttackInterval:
            case EFightProperty.modifyAttackInterval:
                ui.RefreshAttackInterval(GetFightProperty(EFightProperty.calAttackInterval));
                break;
            case EFightProperty.basicAttackCritRate:
            case EFightProperty.modifyAttackCritRate:
                ui.RefreshAttackCritRate((int)GetFightProperty(EFightProperty.calAttackCritRate));
                break;
            case EFightProperty.basicMissRate:
            case EFightProperty.modifyMissRate:
                ui.RefreshAttackedMissingRate((int)GetFightProperty(EFightProperty.calMissRate));
                break;
            default:
                break;
        }
    }

    #endregion [Fight Property Method]

    public void SetId(int id)
    {
        this.id = id;
    }

    public void TryAttack()
    {
        if (!alive) return;

        if (compBuff.HasBuffByType(EBuffType.Stun, out _)) return;

        if (_lastAttackTime == 0 || Time.time > _lastAttackTime + GetFightProperty(EFightProperty.calAttackInterval))
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
        if (randomVal < targetMonster.GetFightProperty(EFightProperty.calMissRate))
        {
            isMissing = true;
            targetMonster.OnAttackedMissing();
            return;
        }

        // 计算暴击
        bool isCritical = false;
        randomVal = Random.Range(0, 100);
        if(randomVal <= GetFightProperty(EFightProperty.calAttackCritRate))
        {
            isCritical = true;
        }

        var dmgVal = (int)GetFightProperty(EFightProperty.calAttack);

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
        var curHp = Mathf.Max(0, GetFightProperty(EFightProperty.curHp) - dmgVal);
        SetFightProperty(EFightProperty.curHp, curHp);

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
        //var randomVal = Random.Range(102, 106);
        //float randomVal2 = Random.Range(0, 10);

        //if (randomVal == 103)
        //{
        //    randomVal2 = Random.Range(-0.2f, 0.2f);
        //}

        //SetFightProperty(
        //    (EFightProperty)randomVal,
        //    GetFightProperty((EFightProperty)randomVal) + randomVal2
        //    );

        if(ManagerTalentPool.Instance.talentPool.TryGetValue(id, out var talent))
        {
            AddBuff(talent.buffDataId);
        }
    }

    public void AddBuff(int dataId)
    {
        compBuff.AddBuff(dataId, id);
    }

    private void OnDead() { }



}
