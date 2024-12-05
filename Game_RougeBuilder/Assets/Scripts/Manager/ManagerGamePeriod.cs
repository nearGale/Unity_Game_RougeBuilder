using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ�׶ο��� Manager
/// </summary>
public class ManagerGamePeriod : Singleton<ManagerGamePeriod>, IManager
{
    public List<GamePeriod> periods = new();
    public int curPeriodIdx;

    private float _periodEnterTime;

    #region Manager Func

    public void Start() 
    {
        CreatePeriods();
    }

    public void Update() 
    {
        if (curPeriodIdx >= periods.Count) return;

        GamePeriod gamePeriod;
        if (curPeriodIdx == -1)
        {
            curPeriodIdx = 0;
            gamePeriod = periods[curPeriodIdx];
            OnEnterPeriod(gamePeriod);
        }

        gamePeriod = periods[curPeriodIdx];
        OnUpdatePeriod(gamePeriod);

        PeriodCheckShallPass(gamePeriod);
    }

    public void FixedUpdate() { }

    public void Reset()
    {
        curPeriodIdx = -1;
    }

    #endregion Manager Func

    public EGamePeriodType GetCurPeriodType()
    {
        if(curPeriodIdx < 0 || curPeriodIdx >= periods.Count)
        {
            return EGamePeriodType.None;
        }

        var gamePeriod = periods[curPeriodIdx];
        return gamePeriod.eType;
    }

    private void CreatePeriods()
    {
        // �׶�1���Դ� 15 ����
        periods.Add(new GamePeriod()
        {
            id = 0,
            eType = EGamePeriodType.Fight,
            ePassCondition = EGamePeriodPassCondition.Time,
            passValue = 5f
        });

        // �׶�2��ѡ���츳
        periods.Add(new GamePeriod()
        {
            id = 1,
            eType = EGamePeriodType.SelectTalent,
            ePassCondition = EGamePeriodPassCondition.TalentPoolCount,
            passValue = 0
        });

        // �׶�1���Դ� 15 ����
        periods.Add(new GamePeriod()
        {
            id = 0,
            eType = EGamePeriodType.Fight,
            ePassCondition = EGamePeriodPassCondition.Time,
            passValue = 5f
        });

        // �׶�2��ѡ���츳
        periods.Add(new GamePeriod()
        {
            id = 1,
            eType = EGamePeriodType.SelectTalent,
            ePassCondition = EGamePeriodPassCondition.TalentPoolCount,
            passValue = 0
        });

        // �׶�1���Դ� 15 ����
        periods.Add(new GamePeriod()
        {
            id = 0,
            eType = EGamePeriodType.Fight,
            ePassCondition = EGamePeriodPassCondition.Time,
            passValue = 5f
        });

        // �׶�2��ѡ���츳
        periods.Add(new GamePeriod()
        {
            id = 1,
            eType = EGamePeriodType.SelectTalent,
            ePassCondition = EGamePeriodPassCondition.TalentPoolCount,
            passValue = 0
        });


        // �׶�1���Դ� 9999 ����
        periods.Add(new GamePeriod()
        {
            id = 0,
            eType = EGamePeriodType.Fight,
            ePassCondition = EGamePeriodPassCondition.Time,
            passValue = 9999f
        });
    }

    private void OnEnterPeriod(GamePeriod gamePeriod)
    {
        _periodEnterTime = Time.time;

        switch (gamePeriod.eType)
        {
            case EGamePeriodType.Fight:
                break;
            case EGamePeriodType.SelectTalent:
                // ���������츳
                ManagerTalentPool.Instance.CreateTalents();
                break;
            default:
                break;
        }
    }

    private void OnUpdatePeriod(GamePeriod gamePeriod)
    {
        switch (gamePeriod.eType)
        {
            case EGamePeriodType.Fight:
                break;
            case EGamePeriodType.SelectTalent:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// �������׶��Ƿ����ͨ����������һ�׶�
    /// </summary>
    /// <param name="gamePeriod"></param>
    private void PeriodCheckShallPass(GamePeriod gamePeriod)
    {
        var shallPass = false;

        switch (gamePeriod.ePassCondition)
        {
            case EGamePeriodPassCondition.Time:
                if(Time.time > _periodEnterTime + gamePeriod.passValue)
                {
                    shallPass = true;
                }
                break;
            case EGamePeriodPassCondition.TalentPoolCount:
                //����츳������
                var count = ManagerTalentPool.Instance.talentPool.Count;
                if (count <= gamePeriod.passValue)
                {
                    shallPass = true;
                }
                break;
        }

        // ���������һ���׶�
        if (shallPass)
        {
            curPeriodIdx++;
            if (curPeriodIdx < periods.Count)
            {
                var nextPeriod = periods[curPeriodIdx];
                OnEnterPeriod(nextPeriod);
            }
        }
    }
}
