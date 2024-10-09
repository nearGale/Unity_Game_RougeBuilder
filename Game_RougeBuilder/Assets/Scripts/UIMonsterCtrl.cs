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
