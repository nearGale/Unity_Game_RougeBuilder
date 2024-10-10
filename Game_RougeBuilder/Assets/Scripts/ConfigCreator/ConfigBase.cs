using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public abstract class ConfigBase : ScriptableObject
{
    public abstract void ParseConfig();

    /// <summary> 转换后数据 </summary>
    public Dictionary<int, DataConfigBase> dictDatas = new();

    public T GetDataById<T>(int dataId) where T: DataConfigBase
    {
        if(dictDatas.TryGetValue(dataId, out var data))
        {
            return data as T;
        }

        return null;
    }
}
