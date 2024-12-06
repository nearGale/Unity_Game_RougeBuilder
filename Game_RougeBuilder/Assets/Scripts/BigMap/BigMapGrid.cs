using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMapGrid 
{
    /// <summary>
    /// 地格ID
    /// </summary>
    public int id;

    public (int, int) xz;

    /// <summary>
    /// 是否是障碍地形
    /// </summary>
    public bool isBlock;

    public BigMapGrid(int id, int x, int z, BigMapGridStaticData gridData)
    {
        this.id = id;
        xz = (x, z);

        isBlock = false;
        if (gridData != null && gridData.eGridType == EBigMapGridType.Wall)
        {
            isBlock = true;
        }
    }

    public override string ToString()
    {
        var str = $"G[{id}|{xz}|{isBlock}]";
        return str;
    }
}
