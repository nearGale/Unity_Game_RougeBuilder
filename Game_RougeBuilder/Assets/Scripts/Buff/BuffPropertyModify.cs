using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 修改战斗属性
/// </summary>
public struct BuffPropModifyParam
{
    public EFightProperty property;
    public float modifyVal;
}

public class BuffPropertyModify : Buff
{
    public List<BuffPropModifyParam> modifyParams = new();

    public override void OnAdd()
    {
        base.OnAdd();

        var monster = ManagerMonster.Instance.GetMonsterById(ownerMonsterId);
        if (monster == null) return;

        foreach(var param in modifyParams)
        {
            monster.SetFightProperty(
                param.property,
                monster.GetFightProperty(param.property) + param.modifyVal);
        }
    }

    public override void OnRemove()
    {
        base.OnRemove();

        var monster = ManagerMonster.Instance.GetMonsterById(ownerMonsterId);
        if (monster == null) return;

        foreach (var param in modifyParams)
        {
            monster.SetFightProperty(
                param.property,
                monster.GetFightProperty(param.property) - param.modifyVal);
        }
    }

    public override void TranslateParamList()
    {
        base.TranslateParamList();

        modifyParams.Clear();
        for (int i = 0; i < listParams.Count; i++)
        {
            if (i % 2 == 0) continue;

            // EFightParam, val, EFightParam, val, ...
            // 两个一对

            var modifyParam = new BuffPropModifyParam()
            {
                property = (EFightProperty)listParams[i - 1],
                modifyVal = listParams[i],
            };
            modifyParams.Add(modifyParam);
        }
    }
}
