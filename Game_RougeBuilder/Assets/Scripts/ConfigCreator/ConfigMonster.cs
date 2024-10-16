using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ConfigAsset/Config Monster", order = 1)]
public class ConfigMonster : ConfigBase
{
    /// <summary> 原始数据 </summary>
    public List<DataMonster> originData;

    public override void ParseConfig()
    {
        dictDatas.Clear();
        foreach (var data in originData)
        {
            dictDatas.Add(data.dataId, data);
        }
    }
}
