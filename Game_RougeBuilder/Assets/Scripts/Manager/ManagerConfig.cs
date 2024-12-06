using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerConfig : Singleton<ManagerConfig>, IManager
{
    public Dictionary<Type, object> configs = new();

    #region Manager Func

    public void Start() 
    {
    }

    public void Update() { }

    public void FixedUpdate() { }

    public void Reset() { }

    #endregion Manager Func


    #region ====== Excel ====== 

    /// <summary>
    /// 读取Excel表格
    /// </summary>
    public void LoadExcel()
    {
        MonsterExcelData monsterExcelData = Resources.Load<MonsterExcelData>("ExcelAsset/MonsterExcelData");
        if (monsterExcelData != null)
        {
            configs.Add(typeof(MonsterExcelData), monsterExcelData);

            foreach (var monster in monsterExcelData.items)
            {
                monster.OnLoad();
            }
        }

        BuffExcelData buffExcelData = Resources.Load<BuffExcelData>("ExcelAsset/BuffExcelData");
        if (buffExcelData != null)
        {
            configs.Add(typeof(BuffExcelData), buffExcelData);

            foreach (var buff in buffExcelData.items)
            {
                buff.OnLoad();
            }
        }
    }

    public T GetExcel<T>() where T : ScriptableObject
    {
        if(configs.TryGetValue(typeof(T), out object config))
        {
            return config as T;
        }

        return null;
    }

    #endregion ====== Excel ====== 
}
