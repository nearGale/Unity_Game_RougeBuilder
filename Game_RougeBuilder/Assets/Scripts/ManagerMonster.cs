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
        Reset();
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
        dictMonsters.Clear();
        newCreateId = 0;
    }

    public MonsterComponent GetMonsterById(int id)
    {
        if(dictMonsters.TryGetValue(id, out var comp))
        {
            return comp;
        }

        return null;
    }

    public (int, MonsterComponent) CreateMonster()
    {
        newCreateId++;

        var monsterComp = new MonsterComponent(
            hpMax: 500, 
            attack: 1, 
            attackInterval: 0.2f,
            attackCriticalRate: 10,
            missRate: 10);

        var ui = GameObject.Instantiate(GameFlowCtrl.Instance.prefabMonsterUI, GameFlowCtrl.Instance.rootMonstersUI);
        var uiMonsterCtrl = ui.GetComponent<UIMonsterCtrl>();

        monsterComp.ui = uiMonsterCtrl;
        monsterComp.Reset(newCreateId);

        dictMonsters.Add(newCreateId, monsterComp);

        return (newCreateId, monsterComp);
    }
}
