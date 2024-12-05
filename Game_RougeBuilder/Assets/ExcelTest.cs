using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelTest : MonoBehaviour
{
    void Start()
    {
        MonsterExcelData monsterExcelData = Resources.Load<MonsterExcelData>("ExcelAsset/MonsterExcelData");
        if (monsterExcelData != null)
        {
            for (int i = 0; i < monsterExcelData.items.Length; i++)
            {
                Debug.Log(monsterExcelData.items[i].ToString());
            }
        }

        BuffExcelData buffExcelData = Resources.Load<BuffExcelData>("ExcelAsset/BuffExcelData");
        if (buffExcelData != null)
        {
            for (int i = 0; i < buffExcelData.items.Length; i++)
            {
                Debug.Log(buffExcelData.items[i].ToString());
            }
        }
    }
}