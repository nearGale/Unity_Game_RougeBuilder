using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ManagerBigMap : Singleton<ManagerBigMap>, IManager
{
    /// <summary> 地格列表，下标和地格id对应 </summary>
    public List<BigMapGrid> grids = new();

    /// <summary> 地图静态配置数据 </summary>
    public BigMapStaticData staticMapData = new();

    #region Manager Func

    public void Start() { }

    public void Update() { }

    public void FixedUpdate() { }

    public void Reset() 
    {
        grids.Clear();
        staticMapData.Clear();
    }

    #endregion Manager Func


    /// <summary>
    /// 读取地图表
    /// </summary>
    public void LoadConfig()
    {
        staticMapData.LoadConfig();
    }


    /// <summary>
    /// 根据表数据创建地图
    /// </summary>
    public void CreateMapGrids()
    {
        grids.Clear();
        
        int id = 0;

        var log = "";
        for(int x = 0; x < staticMapData.mapWidth; x++)
        {
            if(x > 0)
                log += "\n";

            for (int z = 0; z < staticMapData.mapHeight; z++)
            {
                var gridData = staticMapData.GetGridData(x, z);
                BigMapGrid grid = new (id, x, z, gridData);
                grids.Add(grid);

                log += $"{grid.ToString()}  ";
            }
        }

        Debug.Log(log);
    }
}
