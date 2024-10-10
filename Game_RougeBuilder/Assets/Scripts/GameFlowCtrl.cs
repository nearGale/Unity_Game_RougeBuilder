using Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowCtrl : MonoSingleton<GameFlowCtrl>
{
    public GameObject prefabMonsterUI;
    public Transform rootMonstersUI;
    public GameObject prefabTalentUI;
    public Transform rootTalentsUI;
    public Button btnPlay;

    private void Start()
    {
        btnPlay.onClick.AddListener(() =>
        {
            ManagerGamePeriod.Instance.Reset();
            ManagerMonster.Instance.Reset();
        });

        ManagerConfig.Create();
        ManagerGamePeriod.Create();
        ManagerMonster.Create();
        ManagerTalentPool.Create();

        ManagerConfig.Instance.Start();
        ManagerGamePeriod.Instance.Start();
        ManagerMonster.Instance.Start();
        ManagerTalentPool.Instance.Start();

        //var (idA, monsterA) = ManagerMonster.Instance.CreateMonster(
        //    hpMax: 1500,
        //    attack: 26,
        //    attackInterval: 0.2f,
        //    attackCriticalRate: 25,
        //    missRate: 40);
        //var (idB, monsterB) = ManagerMonster.Instance.CreateMonster(
        //    hpMax: 8000,
        //    attack: 13,
        //    attackInterval: 0.5f,
        //    attackCriticalRate: 5,
        //    missRate: 3);

        var (idA, monsterA) = ManagerMonster.Instance.CreateMonster(1);
        var (idB, monsterB) = ManagerMonster.Instance.CreateMonster(2);

        monsterA.targetMonsterId = idB;
        monsterB.targetMonsterId = idA;
    }

    private void Update()
    {
        ManagerGamePeriod.Instance.Update();
        ManagerMonster.Instance.Update();
        ManagerTalentPool.Instance.Update();
    }

    private void FixedUpdate()
    {
        ManagerMonster.Instance.FixedUpdate();
    }
}
