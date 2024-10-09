using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterComponent
{
    #region [Fight Property]

    public int hpCur;

    public int hpMax;

    /// <summary>
    /// 攻击
    /// </summary>
    public int attack;

    /// <summary>
    /// 攻击间隔 (s)
    /// </summary>
    public float attackInterval;

    /// <summary>
    /// 攻击暴击率 (0~100), 计算时 randomVal <= rate 为暴击
    /// </summary>
    public int attackCriticalRate;

    /// <summary>
    /// 闪避率 (0~100), 计算时 randomVal <= rate 为闪避成功
    /// </summary>
    public int missRate;

    #endregion [Fight Property]

    #region [Other Property]

    public int id;

    public bool alive => hpCur > 0;

    private float _lastAttackTime;

    public int targetMonsterId;

    public UIMonsterCtrl ui;

    #endregion [Other Property]

    #region [Life Circle Method]

    public MonsterComponent(int hpMax, int attack, float attackInterval,
        int attackCriticalRate, int missRate)
    {
        this.hpMax = hpMax;
        this.attack = attack;
        this.attackInterval = attackInterval;
        this.attackCriticalRate = attackCriticalRate;
        this.missRate = missRate;
    }

    public void Update()
    {
        if (!alive) return;

        if (_lastAttackTime == 0 || Time.time > _lastAttackTime + attackInterval)
        {
            _lastAttackTime = Time.time;

            DoAttack();
        }
    }

    public void Reset(int id)
    {
        this.id = id;
        hpCur = hpMax;

        ui.RefreshName($"Monster_{id}");
        ui.RefreshHp(hpCur, hpMax);
    }

    #endregion [Life Circle Method]

    private void DoAttack()
    {
        if (targetMonsterId == 0) return;

        var targetMonster = ManagerMonster.Instance.GetMonsterById(targetMonsterId);
        if (targetMonster == null) return;
        if (!targetMonster.alive) return;

        // 计算闪避
        bool isMissing = false;
        int randomVal = Random.Range(0, 100);
        if (randomVal <= targetMonster.missRate)
        {
            isMissing = true;
            targetMonster.OnAttackedMissing();
        }

        // 计算暴击
        bool isCritical = false;
        randomVal = Random.Range(0, 100);
        if(randomVal <= attackCriticalRate)
        {
            isCritical = true;
        }

        var dmgVal = attack;

        if (isCritical)
        {
            dmgVal = (int)(attack * GlobalParams.MonsterAttackCritDmgMultiplier);
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
        hpCur -= dmgVal;

        if (hpCur <= 0)
        {
            hpCur = 0;
            OnDead();
        }

        ui.ShowDmgTaken(dmgVal, isCritical);
        ui.RefreshHp(hpCur, hpMax);
    }

    /// <summary>
    /// 当自己闪避了敌方攻击时
    /// </summary>
    public void OnAttackedMissing()
    {
        ui.ShowAttackedMissing();
    }

    private void OnDead() { }


}
