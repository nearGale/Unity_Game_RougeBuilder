using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏阶段控制 Manager
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
        // 阶段1：对打 15 秒钟
        periods.Add(new GamePeriod()
        {
            id = 0,
            eType = EGamePeriodType.Fight,
            ePassCondition = EGamePeriodPassCondition.Time,
            passValue = 5f
        });

        // 阶段2：选择天赋
        periods.Add(new GamePeriod()
        {
            id = 1,
            eType = EGamePeriodType.SelectTalent,
            ePassCondition = EGamePeriodPassCondition.TalentPoolCount,
            passValue = 0
        });

        // 阶段1：对打 15 秒钟
        periods.Add(new GamePeriod()
        {
            id = 0,
            eType = EGamePeriodType.Fight,
            ePassCondition = EGamePeriodPassCondition.Time,
            passValue = 5f
        });

        // 阶段2：选择天赋
        periods.Add(new GamePeriod()
        {
            id = 1,
            eType = EGamePeriodType.SelectTalent,
            ePassCondition = EGamePeriodPassCondition.TalentPoolCount,
            passValue = 0
        });

        // 阶段1：对打 15 秒钟
        periods.Add(new GamePeriod()
        {
            id = 0,
            eType = EGamePeriodType.Fight,
            ePassCondition = EGamePeriodPassCondition.Time,
            passValue = 5f
        });

        // 阶段2：选择天赋
        periods.Add(new GamePeriod()
        {
            id = 1,
            eType = EGamePeriodType.SelectTalent,
            ePassCondition = EGamePeriodPassCondition.TalentPoolCount,
            passValue = 0
        });


        // 阶段1：对打 9999 秒钟
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
                // 出现六个天赋
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
    /// 检查这个阶段是否可以通过，进入下一阶段
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
                //检查天赋池数量
                var count = ManagerTalentPool.Instance.talentPool.Count;
                if (count <= gamePeriod.passValue)
                {
                    shallPass = true;
                }
                break;
        }

        // 允许进入下一个阶段
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
