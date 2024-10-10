using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public EBuffType eType;

    public int dataId;

    public int ownerMonsterId;

    public float restTime; // -1 为永久，>0 为限时

    public List<float> listParams; // 配表的参数

    /// <summary>
    /// Buff Update逻辑
    /// </summary>
    /// <returns>这个buff是否继续存在</returns>
    public bool LogicUpdate()
    {
        if (restTime == -1)
        {
            return true;
        }
        
        if (restTime > 0)
        {
            restTime -= Time.fixedDeltaTime;
            return restTime > 0;
        }

        return false;
    }

    public virtual void OnAdd() { }

    public virtual void OnRemove() { }

    /// <summary>
    /// 将表里的参数数据，转成此类型的实际用法
    /// </summary>
    public virtual void TranslateParamList() { }
}
