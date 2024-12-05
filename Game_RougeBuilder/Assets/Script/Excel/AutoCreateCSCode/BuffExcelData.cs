/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

[Serializable]
public partial class BuffExcelItem : ExcelItemBase
{
	public string name;
	public string desc;
	public EBuffType buffType;
	public float restTime;
	public List<float> paramList;
	public List<string> strParams;
}

[CreateAssetMenu(fileName = "BuffExcelData", menuName = "Excel To ScriptableObject/Create BuffExcelData", order = 1)]
public class BuffExcelData : ExcelDataBase<BuffExcelItem>
{
}

#if UNITY_EDITOR
public class BuffAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		BuffExcelItem[] items = new BuffExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new BuffExcelItem();
			items[i].id = Convert.ToInt32(allItemValueRowList[i]["id"]);
			items[i].name = allItemValueRowList[i]["name"];
			items[i].desc = allItemValueRowList[i]["desc"];
			items[i].buffType = Enum.Parse<EBuffType>(allItemValueRowList[i]["buffType"]);
			items[i].restTime = Convert.ToSingle(allItemValueRowList[i]["restTime"]);
			items[i].paramList = allItemValueRowList[i]["paramList"] == null ? new() : allItemValueRowList[i]["paramList"].Split(';').Select(x => Convert.ToSingle(x)).ToList();
			items[i].strParams = allItemValueRowList[i]["strParams"] == null ? new() : allItemValueRowList[i]["strParams"].Split(';').ToList();
		}
		BuffExcelData excelDataAsset = ScriptableObject.CreateInstance<BuffExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(BuffExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


