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
    /// 受到伤害prefab
    /// </summary>
    public GameObject damageTakenPrefab;

    public Transform rootAttackedMissing;

    /// <summary>
    /// 闪避攻击prefab
    /// </summary>
    public GameObject attackedMissingPrefab;

    /// <summary>
    /// 受击的时间，过[damageTakenShowTime]秒后置空
    /// </summary>
    private float damageTakenTime;

    /// <summary>
    /// 受击显示时长 (s)
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
    /// 显示自己受到伤害
    /// </summary>
    public void ShowDmgTaken(int dmgTaken, bool isCritical)
    {
        var dmgTakenGo = Instantiate(damageTakenPrefab, rootDmgTaken);
        var dmgTakenCtrl = dmgTakenGo.GetComponent<UIMonsterDmgTakenCtrl>();

        dmgTakenCtrl.ShowDmgTaken(dmgTaken, isCritical);
    }

    /// <summary>
    /// 显示自己闪避了敌方攻击
    /// </summary>
    public void ShowAttackedMissing()
    {
        var missingGo = Instantiate(attackedMissingPrefab, rootAttackedMissing);

        var missingCtrl = missingGo.GetComponent<UIMonsterAttackedMissingCtrl>();
        missingCtrl.SetStartPosX();
    }
}
