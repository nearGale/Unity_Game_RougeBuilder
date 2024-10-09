using Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowCtrl : MonoSingleton<GameFlowCtrl>
{
    public GameObject prefabMonsterUI;
    public Transform rootMonstersUI;

    private void Start()
    {
        ManagerGameFlow.Create();
        ManagerMonster.Create();
        ManagerGameFlow.Instance.Start();
        ManagerMonster.Instance.Start();

        var (idA, monsterA) = ManagerMonster.Instance.CreateMonster();
        var (idB, monsterB) = ManagerMonster.Instance.CreateMonster();

        monsterA.targetMonsterId = idB;
        monsterB.targetMonsterId = idA;
    }

    private void Update()
    {
        ManagerGameFlow.Instance.Update();
        ManagerMonster.Instance.Update();
    }
}
