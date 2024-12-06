/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

[Serializable]
public partial class MonsterExcelItem : ExcelItemBase
{
	public string name;
	public EMonsterType eType;
	public float basicHp;
	public float basicAttack;
	public float basicDefender;
	public float attackInterval;
	public float attackCritRate;
	public float missRate;
	public List<int> carringBuff;
}

[CreateAssetMenu(fileName = "MonsterExcelData", menuName = "Excel To ScriptableObject/Create MonsterExcelData", order = 1)]
public class MonsterExcelData : ExcelDataBase<MonsterExcelItem>
{
}

#if UNITY_EDITOR
public class MonsterAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		MonsterExcelItem[] items = new MonsterExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new MonsterExcelItem();
			items[i].id = Convert.ToInt32(allItemValueRowList[i]["id"]);
			items[i].name = allItemValueRowList[i]["name"];
			items[i].eType = Enum.Parse<EMonsterType>(allItemValueRowList[i]["eType"]);
			items[i].basicHp = Convert.ToSingle(allItemValueRowList[i]["basicHp"]);
			items[i].basicAttack = Convert.ToSingle(allItemValueRowList[i]["basicAttack"]);
			items[i].basicDefender = Convert.ToSingle(allItemValueRowList[i]["basicDefender"]);
			items[i].attackInterval = Convert.ToSingle(allItemValueRowList[i]["attackInterval"]);
			items[i].attackCritRate = Convert.ToSingle(allItemValueRowList[i]["attackCritRate"]);
			items[i].missRate = Convert.ToSingle(allItemValueRowList[i]["missRate"]);
			items[i].carringBuff = allItemValueRowList[i]["carringBuff"] == null ? new() : allItemValueRowList[i]["carringBuff"].Split(';').Select(x => Convert.ToInt32(x)).ToList();
		}
		MonsterExcelData excelDataAsset = ScriptableObject.CreateInstance<MonsterExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(MonsterExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


