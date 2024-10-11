using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataConfigBase
{
    public int dataId;
}


[Serializable]
public class DataMonster : DataConfigBase
{
    public string name;
    public float attack;
    public float defense;
    public float hp;
    public float attackInterval;
    public float attackCritRate;
    public float missRate;
    public List<int> carringBuff;
}

[Serializable]
public class DataBuff : DataConfigBase
{
    public string name;
    public string desc;
    public EBuffType eBuffType;
    public float restTime;
    public List<float> paramList;
}