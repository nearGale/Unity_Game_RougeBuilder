using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UIMonsterCtrl : MonoBehaviour
{
    public Slider testSlider;
    public Text textHp;
    public Text textName;
    public Text textAttack;
    public Text textAttackInterval;
    public Text textAttackCritRate;
    public Text textAttackedMissingRate;
    public Text textTalents;
    public Transform rootDmgTaken;

    /// <summary>
    /// �ܵ��˺�prefab
    /// </summary>
    public GameObject damageTakenPrefab;

    public Transform rootAttackedMissing;

    /// <summary>
    /// ���ܹ���prefab
    /// </summary>
    public GameObject attackedMissingPrefab;

    /// <summary>
    /// �ܻ���ʱ�䣬��[damageTakenShowTime]����ÿ�
    /// </summary>
    private float damageTakenTime;

    /// <summary>
    /// �ܻ���ʾʱ�� (s)
    /// </summary>
    private float damageTakenShowTime = 1f;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void RefreshName(string name)
    {
        textName.text = name;
    }

    public void RefreshHp(int curHp, int maxHp)
    {
        textHp.text = $"{curHp} / {maxHp}";

        testSlider.minValue = 0;
        testSlider.maxValue = maxHp;
        testSlider.value = curHp;
    }

    public void RefreshAttack(int val)
    {
        textAttack.text = $"������: {val}";
    }

    public void RefreshAttackInterval(float val)
    {
        textAttackInterval.text = val.ToString("�������: #0.00s");
    }

    public void RefreshAttackCritRate(int val)
    {
        textAttackCritRate.text = $"������: {val}%";
    }

    public void RefreshAttackedMissingRate(int val)
    {
        textAttackedMissingRate.text = $"������: {val}%";
    }

    public void RefreshTalents(List<int> talents)
    {
        textTalents.text = $"�츳: {String.Join(',', talents)}";
    }

    /// <summary>
    /// ��ʾ�Լ��ܵ��˺�
    /// </summary>
    public void ShowDmgTaken(int dmgTaken, bool isCritical)
    {
        var dmgTakenGo = Instantiate(damageTakenPrefab, rootDmgTaken);
        var dmgTakenCtrl = dmgTakenGo.GetComponent<UIMonsterDmgTakenCtrl>();

        dmgTakenCtrl.ShowDmgTaken(dmgTaken, isCritical);
    }

    /// <summary>
    /// ��ʾ�Լ������˵з�����
    /// </summary>
    public void ShowAttackedMissing()
    {
        var missingGo = Instantiate(attackedMissingPrefab, rootAttackedMissing);

        var missingCtrl = missingGo.GetComponent<UIMonsterAttackedMissingCtrl>();
        missingCtrl.SetStartPosX();
    }
}
