using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ManagerMonster : Singleton<ManagerMonster>, IManager
{
    public Dictionary<int, Monster> dictMonsters = new();

    private int newCreateId = 0;

    #region Manager Func

    public void Start()
    {
        dictMonsters.Clear();
        newCreateId = 0;
    }

    public void Update() { }

    public void FixedUpdate()
    {
        if (ManagerGamePeriod.Instance.GetCurPeriodType() != EGamePeriodType.Fight) return;

        // 相互攻击
        foreach (var kvPair in dictMonsters)
        {
            var monsterComp = kvPair.Value;
            monsterComp.TryAttack();
        }

        // TODO: Update Buff
        foreach (var kvPair in dictMonsters)
        {
            var monsterComp = kvPair.Value;
            monsterComp.BuffLogicUpdate();
        }
    }

    public void Reset()
    {
        foreach (var kvPair in dictMonsters)
        {
            var monsterComp = kvPair.Value;
            monsterComp.Reset();
        }
    }

    #endregion Manager Func

    public Monster GetMonsterById(int id)
    {
        if(dictMonsters.TryGetValue(id, out var comp))
        {
            return comp;
        }

        return null;
    }

    public (int, Monster) CreateMonster(int dataId)
    {
        newCreateId++;

        var configMonster = ManagerConfig.Instance.Get<MonsterExcelData>();
        var dataMonster = configMonster.GetExcelItem(dataId);

        if (dataMonster == null) return (0, null);

        var monsterComp = new Monster();

        var ui = GameObject.Instantiate(GameFlowCtrl.Instance.prefabMonsterUI, GameFlowCtrl.Instance.rootMonstersUI);
        var uiMonsterCtrl = ui.GetComponent<UIMonsterCtrl>();

        monsterComp.ui = uiMonsterCtrl;
        monsterComp.Reset();

        monsterComp.SetId(newCreateId);
        monsterComp.SetBasicProperty(dataMonster);


        dictMonsters.Add(newCreateId, monsterComp);

        return (newCreateId, monsterComp);
    }
}
