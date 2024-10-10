using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBuff
{
    public List<Buff> buffs = new();

    private List<Buff> _tmpList = new();

    public void Clear()
    {
        buffs.Clear();
        _tmpList.Clear();
    }

    public void LogicUpdate()
    {
        _tmpList.Clear();

        int idx = 0;
        foreach (var buff in buffs)
        {
            var stillAlive = buff.LogicUpdate();
            if (!stillAlive)
            {
                _tmpList.Add(buff);
            }
            idx++;
        }

        foreach (var buff in _tmpList)
        {
            buff.OnRemove();
            buffs.Remove(buff);
        }

        _tmpList.Clear();
    }

    public void AddBuff(int dataId, int ownerId, float restTime)
    {
        var buff = new Buff();
        buff.dataId = dataId;
        buff.ownerMonsterId = ownerId;
        buff.restTime = restTime;

        buffs.Add(buff);
        buff.OnAdd();
    }
}
