using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public int dataId;

    public int ownerMonsterId;

    public float restTime; // -1 为永久，>0 为限时

    /// <summary>
    /// Buff Update逻辑
    /// </summary>
    /// <returns>这个buff是否继续存在</returns>
    public bool LogicUpdate()
    {
        var skillAlive = true;

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

    public void OnAdd()
    {
        var monster = ManagerMonster.Instance.GetMonsterById(ownerMonsterId);
        if (monster != null)
        {
            monster.SetFightProperty(
                EFightProperty.attack_Modify,
                monster.GetFightProperty(EFightProperty.attack_Modify) + 10);
        }
    }

    public void OnRemove()
    {
        var monster = ManagerMonster.Instance.GetMonsterById(ownerMonsterId);
        if (monster != null)
        {
            monster.SetFightProperty(
                EFightProperty.attack_Modify,
                monster.GetFightProperty(EFightProperty.attack_Modify) - 10);
        }
    }
}
