using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorConfigBuff : EditorWindow
{
    private static EditorConfigBuff window; //窗体实例

    private int _curDataId;

    private ConfigBuff _config;

    /// <summary>
    /// 数据中的 data
    /// </summary>
    private DataBuff _originData;

    //显示窗体
    [MenuItem("Tools/Config Editor/Buff")]
    private static void ShowWindow()
    {
        window = EditorWindow.GetWindow<EditorConfigBuff>("Config Editor - Buff");
        window.position = new Rect(500, 300, 500f, 400f);
        window.Show();
    }

    //显示时调用
    private void OnEnable()
    {
        _config = Resources.Load<ConfigBuff>("Config/ConfigBuff");
        Reset();
    }

    //固定帧数调用
    private void Update()
    {
    }

    //隐藏时调用
    private void OnDisable()
    {
    }

    //销毁时调用
    private void OnDestroy()
    {
    }

    private void Reset()
    {
        _curDataId = 0;
        _originData = null;
    }

    //绘制窗体内容
    private void OnGUI()
    {
        if (GUILayout.Button("Create New"))
        {
            DataBuff data = new DataBuff();
            var lastDataId = _config.originData[_config.originData.Count - 1].dataId;
            data.dataId = lastDataId + 1;
            _config.originData.Add(data);

            OnDataIdSelected(data.dataId);
        }

        if (GUILayout.Button("Delete This"))
        {
            _config.originData.Remove(_originData);
            Reset();
        }

        // 选择ID
        // 下拉框 wiki: https://docs.unity3d.com/cn/2017.2/ScriptReference/EditorGUILayout.DropdownButton.html
        if (EditorGUILayout.DropdownButton(new GUIContent("选择 dataId"), FocusType.Keyboard, GUILayout.Width(100)))
        {
            // create the menu and add items to it
            GenericMenu menu = new GenericMenu();

            foreach (var item in _config.originData)
            {
                AddMenuItemForBuff(menu, $"{item.dataId}-{item.name}", item.dataId);
            }

            // display the menu
            menu.ShowAsContext();
        }

        EditorGUILayout.LabelField($"dataId: {_curDataId}", EditorStyles.boldLabel);

        if (_originData != null)
        {
            WindowHelper.IntField("dataId", ref _originData.dataId);
            WindowHelper.StrField("名字", ref _originData.name);
            WindowHelper.StrField("描述", ref _originData.desc);
            WindowHelper.FloatField("持续时间", ref _originData.restTime);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField($"buff类型: ");
            EditorGUILayout.LabelField($"{_originData.eBuffType}");
            if (EditorGUILayout.DropdownButton(new GUIContent("选择"), FocusType.Keyboard, GUILayout.Width(100)))
            {
                // create the menu and add items to it
                GenericMenu menu = new GenericMenu();

                foreach (var item in Enum.GetValues(typeof(EBuffType)))
                {
                    AddMenuItemForEType(menu, $"{(int)item}-{(EBuffType)item}", (EBuffType)item);
                }

                // display the menu
                menu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();


            var a = 1f;
            WindowHelper.FloatField("参数", ref a);

        }
    }

    void AddMenuItemForBuff(GenericMenu menu, string menuPath, int dataId)
    {
        // 如果 item 项与当前选中的 dataId 匹配，则将其标记为选中
        menu.AddItem(new GUIContent(menuPath), _curDataId == dataId, OnDataIdSelected, dataId);
    }

    private void OnDataIdSelected(object dataId)
    {
        _curDataId = (int)dataId;
        _originData = _config.originData.Find(x => x.dataId == _curDataId);
    }

    // 添加下拉框item
    void AddMenuItemForEType(GenericMenu menu, string menuPath, EBuffType eBuffType)
    {
        // 如果 item 项与当前选中的 dataId 匹配，则将其标记为选中
        menu.AddItem(new GUIContent(menuPath), _originData.eBuffType == eBuffType, OnETypeSelected, eBuffType);
    }

    // 选中一个 dataId
    private void OnETypeSelected(object eBuffType)
    {
        _originData.eBuffType = (EBuffType)eBuffType;
    }
}

