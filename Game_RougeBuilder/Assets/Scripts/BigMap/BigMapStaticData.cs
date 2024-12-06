using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
/// 地图静态数据
/// </summary>
public class BigMapStaticData 
{
    public int mapWidth;
    public int mapHeight;

    public Dictionary<(int, int), BigMapGridStaticData> gridInfo = new();

    public void Clear()
    {
        gridInfo.Clear();
        mapWidth = 0;
        mapHeight = 0;
    }

    public void LoadConfig()
    {
        gridInfo.Clear();
        mapWidth = 0;
        mapHeight = 0;

        using (var stream = File.Open(
            Application.dataPath + GlobalParams.ConfigPathBigMap, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                if (result.Tables.Count > 0)
                {
                    for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < result.Tables[0].Columns.Count; j++)
                        {
                            var val = result.Tables[0].Rows[i][j].ToString();
                            BigMapGridStaticData gridData = new(val);
                            gridInfo.Add((i, j), gridData);
                            //Debug.Log(result.Tables[0].Rows[i][j].ToString());
                        }
                    }
                }

                mapWidth = result.Tables[0].Columns.Count;
                mapHeight = result.Tables[0].Rows.Count;
                reader.Close();
            }
        }
    }

    public BigMapGridStaticData GetGridData(int x, int z)
    {
        if(gridInfo.TryGetValue((x, z), out var gridData))
        {
            return gridData;
        }

        return null;
    }
}

/// <summary>
/// 地格静态数据
/// </summary>
public class BigMapGridStaticData
{
    public EBigMapGridType eGridType;
    public float info;

    public BigMapGridStaticData(string val)
    {
        Reset();

        if (val == GlobalParams.BigMapGridStaticDataWall)
        {
            eGridType = EBigMapGridType.Wall;
        }
        else if(val.StartsWith(GlobalParams.BigMapGridStaticDataNPC))
        {
            eGridType = EBigMapGridType.NPC;

            var infoS = val[1..];
            info = Convert.ToInt32(infoS);
        }
        else if (val.StartsWith(GlobalParams.BigMapGridStaticDataMonster))
        {
            eGridType = EBigMapGridType.Monster;

            var infoS = val[1..];
            info = Convert.ToInt32(infoS);
        }
    }

    public void Reset()
    {
        eGridType = EBigMapGridType.None;
        info = 0;
    }
}