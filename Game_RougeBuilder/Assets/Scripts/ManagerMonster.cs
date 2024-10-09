using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ManagerMonster : Singleton<ManagerMonster>
{
    public Dictionary<int, MonsterComponent> dictMonsters = new();

    private int newCreateId = 0;

    public void Start()
    {
        dictMonsters.Clear();
        newCreateId = 0;
    }

    public void Update() 
    {
        foreach(var kvPair in dictMonsters)
        {
            var monsterComp = kvPair.Value;
            monsterComp.Update();
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

    public MonsterComponent GetMonsterById(int id)
    {
        if(dictMonsters.TryGetValue(id, out var comp))
        {
            return comp;
        }

        return null;
    }

    public (int, MonsterComponent) CreateMonster(
        int hpMax, int attack, float attackInterval,
        int attackCriticalRate, int missRate)
    {
        newCreateId++;

        var monsterComp = new MonsterComponent();

        var ui = GameObject.Instantiate(GameFlowCtrl.Instance.prefabMonsterUI, GameFlowCtrl.Instance.rootMonstersUI);
        var uiMonsterCtrl = ui.GetComponent<UIMonsterCtrl>();

        monsterComp.ui = uiMonsterCtrl;
        monsterComp.SetId(newCreateId);
        monsterComp.SetFightProperty(
            hpMax,
            attack,
            attackInterval,
            attackCriticalRate,
            missRate);
        monsterComp.Reset();

        dictMonsters.Add(newCreateId, monsterComp);

        return (newCreateId, monsterComp);
    }
}
