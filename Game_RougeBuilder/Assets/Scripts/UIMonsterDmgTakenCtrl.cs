using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 伤害飘字UI组件
/// </summary>
public class UIMonsterDmgTakenCtrl : MonoBehaviour
{
    public Text textVal;

    private float _showTime;

    private float _showDuration = 1f;

    /// <summary>
    /// 飘字速度
    /// </summary>
    private float _speed;

    void Update()
    {
        textVal.rectTransform.position = textVal.rectTransform.position + new Vector3(0, 0.2f, 0) * _speed;

        if(Time.time > _showTime + _showDuration)
        {
            Destroy(gameObject);
        }
    }

    public void ShowDmgTaken(int dmgTaken, bool isCritical)
    {
        var randomPos = new Vector3(Random.Range(-100, 100), Random.Range(-15, 15), 0);
        textVal.rectTransform.position = textVal.rectTransform.position + randomPos;

        var str = $"-{dmgTaken}";
        textVal.text = str;

        textVal.fontStyle = isCritical ? FontStyle.Bold : FontStyle.Normal;

        _showTime = Time.time;

        _speed = Random.Range(0.1f, 0.3f);
    }

}
