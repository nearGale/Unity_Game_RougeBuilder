using Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowCtrl : MonoSingleton<GameFlowCtrl>
{
    public GameObject prefabMonsterUI;
    public Transform rootMonstersUI;
    public Button btnPlay;

    private void Start()
    {
        btnPlay.onClick.AddListener(() =>
        {
            ManagerMonster.Instance.Reset();
        });

        ManagerGameFlow.Create();
        ManagerMonster.Create();
        ManagerGameFlow.Instance.Start();
        ManagerMonster.Instance.Start();

        var (idA, monsterA) = ManagerMonster.Instance.CreateMonster(
            hpMax: 1500,
            attack: 26,
            attackInterval: 0.2f,
            attackCriticalRate: 25,
            missRate: 40);
        var (idB, monsterB) = ManagerMonster.Instance.CreateMonster(
            hpMax: 8000,
            attack: 13,
            attackInterval: 0.5f,
            attackCriticalRate: 5,
            missRate: 3);

        monsterA.targetMonsterId = idB;
        monsterB.targetMonsterId = idA;
    }

    private void Update()
    {
        ManagerGameFlow.Instance.Update();
        ManagerMonster.Instance.Update();
    }
}
