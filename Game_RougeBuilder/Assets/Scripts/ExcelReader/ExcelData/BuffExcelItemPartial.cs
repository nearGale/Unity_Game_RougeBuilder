using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BuffExcelItem
{
    public List<BuffPropModifyParam> modifyParams = new();

    public override void OnLoad()
    {
        modifyParams.Clear();
        for (int i = 0; i < strParams.Count; i++)
        {
            if (i % 2 == 0) continue;

            // EFightParam, val, EFightParam, val, ...
            // 两个一对

            var modifyParam = new BuffPropModifyParam()
            {
                property = Enum.Parse<EFightProperty>(strParams[i - 1]),
                modifyVal = Convert.ToSingle(strParams[i]),
            };
            modifyParams.Add(modifyParam);
        }
    }

    public override string ToString()
    {
        var str = $"id:{id} " +
            $"{name} " +
            $"desc:{desc} " +
            $"buffType:{buffType} " +
            $"restTime:{restTime} " +
            $"paramList:[{string.Join(',', paramList)}] ";
        return str;
    }
}
