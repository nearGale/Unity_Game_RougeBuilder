using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ConfigAsset/Config Buff", order = 1)]
public class ConfigBuff : ConfigBase
{
    /// <summary> 原始数据 </summary>
    public List<DataBuff> originData;

    public override void ParseConfig()
    {
        dictDatas.Clear();
        foreach (var data in originData)
        {
            dictDatas.Add(data.dataId, data);
        }
    }
}
