using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITalentCtrl : MonoBehaviour
{
    public int id;

    public Text textDesc;
    public Button btnTalent;

    private void Start()
    {
        btnTalent.onClick.AddListener(() =>
        {
            ManagerTalentPool.Instance.OnSelectTalent(id);
        });
    }

    public void SetDesc(string desc)
    {
        textDesc.text = desc;
    }
}
