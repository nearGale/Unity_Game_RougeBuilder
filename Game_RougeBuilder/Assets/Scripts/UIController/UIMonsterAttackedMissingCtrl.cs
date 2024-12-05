using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMonsterAttackedMissingCtrl : MonoBehaviour
{
    public Text ui;

    private float _startPosX;

    /// <summary> �񶯷��� </summary>
    private float _vibrateX = 3f;

    /// <summary> ���ٶ� </summary>
    private float _vibrateSpeed = 1f;

    /// <summary> ���������ƶ� </summary>
    private bool _directionRight = true;

    /// <summary> ���ֵ�ʱ�� </summary>
    private float _showTime;

    /// <summary> ���ֵ�ʱ�� </summary>
    private float _showDuration = 0.4f;

    private void Start()
    {
        _showTime = Time.time;
    }

    private void Update()
    {
        if(Mathf.Abs(ui.rectTransform.position.x - _startPosX) > _vibrateX)
        {
            _directionRight = !_directionRight;
        }

        var sign = _directionRight ? 1 : -1;
        ui.rectTransform.position = ui.rectTransform.position + new Vector3(1, 0, 0) * sign * _vibrateSpeed;

        if(Time.time > _showTime + _showDuration)
        {
            Destroy(gameObject);
        }
    }

    public void SetStartPosX()
    {
        var randomPos = new Vector3(Random.Range(-100, 100), Random.Range(-15, 15), 0);
        ui.rectTransform.position = ui.rectTransform.position + randomPos;

        _startPosX = ui.rectTransform.position.x;
    }
}
