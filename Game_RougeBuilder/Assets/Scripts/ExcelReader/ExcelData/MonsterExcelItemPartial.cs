using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MonsterExcelItem
{
    public override string ToString()
    {
        var str = $"id:{id} " +
            $"{name} " +
            $"type:{eType} " +
            $"hp:{basicHp} " +
            $"atk:{basicAttack} " +
            $"def:{basicDefender} " +
            $"atkInterval:{attackInterval} " +
            $"critRate:{attackCritRate} " +
            $"missRate:{missRate} " +
            $"carringBuff:[{string.Join(',', carringBuff)}] ";
        return str;
    }
}
