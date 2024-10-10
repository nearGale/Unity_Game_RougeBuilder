using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerConfig : Singleton<ManagerConfig>
{
    public Dictionary<Type, ConfigBase> configs = new();

    public void Start() 
    {
        var configMonster = Resources.Load<ConfigMonster>("Config/ConfigMonster");
        configMonster.ParseConfig();
        configs.Add(typeof(ConfigMonster), configMonster);

        var configBuff = Resources.Load<ConfigBuff>("Config/ConfigBuff");
        configBuff.ParseConfig();
        configs.Add(typeof(ConfigBuff), configBuff);

    }

    public T Get<T>() where T : ConfigBase
    {
        if(configs.TryGetValue(typeof(T), out ConfigBase config))
        {
            return config as T;
        }

        return null;
    }

}
