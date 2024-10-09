using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterComponent
{
    #region [Fight Property]

    public int hpCur;

    public int hpMax;

    /// <summary>
    /// ����
    /// </summary>
    public int attack;

    /// <summary>
    /// ������� (s)
    /// </summary>
    public float attackInterval;

    /// <summary>
    /// ���������� (0~100), ����ʱ randomVal <= rate Ϊ����
    /// </summary>
    public int attackCriticalRate;

    /// <summary>
    /// ������ (0~100), ����ʱ randomVal <= rate Ϊ���ܳɹ�
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

        // ��������
        bool isMissing = false;
        int randomVal = Random.Range(0, 100);
        if (randomVal <= targetMonster.missRate)
        {
            isMissing = true;
            targetMonster.OnAttackedMissing();
        }

        // ���㱩��
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
    /// ���Լ��ܵ�����ʱ
    /// </summary>
    /// <param name="dmgVal">�˺���ֵ</param>
    /// <param name="isCritical">�Ƿ��Ǳ���</param>
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
    /// ���Լ������˵з�����ʱ
    /// </summary>
    public void OnAttackedMissing()
    {
        ui.ShowAttackedMissing();
    }

    private void OnDead() { }


}
