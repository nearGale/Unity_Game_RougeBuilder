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

    public void AddBuff(int dataId, int ownerId)
    {
        var configBuff = ManagerConfig.Instance.Get<ConfigBuff>();
        var dataBuff = configBuff.GetDataById<DataBuff>(dataId);

        if (dataBuff == null) return;

        Buff buff = null;

        switch (dataBuff.eBuffType)
        {
            case EBuffType.FightProperty:
                buff = new BuffPropertyModify();
                break;
        }

        buff.dataId = dataId;
        buff.ownerMonsterId = ownerId;
        buff.restTime = dataBuff.restTime;
        buff.listParams = dataBuff.paramList;

        buff.TranslateParamList();

        buffs.Add(buff);
        buff.OnAdd();
    }
}


// TODO: readCSV
public class CSVBuff
{
    public int dataId;
    public EBuffType eType;
    public List<float> paramList = new();
    public float restTime;
}