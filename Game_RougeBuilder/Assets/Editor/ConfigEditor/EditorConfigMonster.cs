using UnityEditor;
using UnityEngine;

public class EditorConfigMonster : EditorWindow
{
    private static EditorConfigMonster window; //窗体实例

    private int _curDataId;

    private ConfigMonster _configMonster;

    private ConfigBuff _configBuff;

    /// <summary>
    /// 数据中的 data
    /// </summary>
    private DataMonster _originData;

    //显示窗体
    [MenuItem("Tools/Config Editor/Monster")]
    private static void ShowWindow()
    {
        window = EditorWindow.GetWindow<EditorConfigMonster>("Config Editor - Monster");
        window.position = new Rect(500, 200f, 500f, 600f);
        window.Show();
    }

    //显示时调用
    private void OnEnable()
    {
        _configMonster = Resources.Load<ConfigMonster>("Config/ConfigMonster");
        
        _configBuff = Resources.Load<ConfigBuff>("Config/ConfigBuff");
        _configBuff.ParseConfig();

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
            DataMonster data = new DataMonster();
            var lastDataId = _configMonster.originData[_configMonster.originData.Count - 1].dataId;
            data.dataId = lastDataId + 1;
            _configMonster.originData.Add(data);

            OnDataIdSelected(data.dataId);
        }

        if (GUILayout.Button("Delete This"))
        {
            _configMonster.originData.Remove(_originData);
            Reset();
        }

        // 选择ID
        // 下拉框 wiki: https://docs.unity3d.com/cn/2017.2/ScriptReference/EditorGUILayout.DropdownButton.html
        if (EditorGUILayout.DropdownButton(new GUIContent("选择 dataId"), FocusType.Keyboard, GUILayout.Width(100)))
        {
            // create the menu and add items to it
            GenericMenu menu = new GenericMenu();

            foreach (var item in _configMonster.originData)
            {
                AddMenuItemForMonster(menu, $"{item.dataId}-{item.name}", item.dataId);
            }

            // display the menu
            menu.ShowAsContext();
        }

        EditorGUILayout.LabelField($"dataId: {_curDataId}", EditorStyles.boldLabel);

        if (_originData != null)
        {
            //var sDataId = EditorGUILayout.TextField("dataId: ", _data.dataId.ToString());
            //_data.dataId = int.Parse(sDataId);
            WindowHelper.IntField("dataId", ref _originData.dataId);
            WindowHelper.StrField("名字", ref _originData.name);

            EditorGUILayout.BeginHorizontal();
            WindowHelper.FloatField("攻击力", ref _originData.attack);
            EditorGUILayout.Space();
            WindowHelper.FloatField("暴击率 (0~100)", ref _originData.attackCritRate);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            WindowHelper.FloatField("防御力", ref _originData.defense);
            EditorGUILayout.Space();
            WindowHelper.FloatField("闪避率 (0~100)", ref _originData.missRate);
            EditorGUILayout.EndHorizontal();

            WindowHelper.FloatField("血量", ref _originData.hp);
            WindowHelper.FloatField("攻击间隔 (s)", ref _originData.attackInterval);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"携带的buff: ");

            if (_originData.carringBuff == null) 
                _originData.carringBuff = new();

            if (GUILayout.Button("新增buff"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (var item in _configBuff.originData)
                {
                    AddMenuItemForBuff(menu, $"{item.dataId}-{item.name}", item.dataId);
                }

                menu.ShowAsContext();
            }

            int buffToRemove = 0;
            foreach (var buffDataId in _originData.carringBuff)
            {
                EditorGUILayout.BeginHorizontal();

                // buff 描述
                var dataBuff = _configBuff.GetDataById<DataBuff>(buffDataId);
                EditorGUILayout.LabelField($"{buffDataId}-{dataBuff.name}\n{dataBuff.desc}", GUILayout.Height(35));

                var strTime = dataBuff.restTime == -1 ? "永久" : $"{dataBuff.restTime}s";
                EditorGUILayout.LabelField($"持续时间: {strTime}");

                // 移除 buff 按钮
                if (GUILayout.Button("移除"))
                {
                    buffToRemove = buffDataId;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (buffToRemove != 0)
                _originData.carringBuff.Remove(buffToRemove);
        }
    }

    // 添加下拉框item
    void AddMenuItemForMonster(GenericMenu menu, string menuPath, int dataId)
    {
        // 如果 item 项与当前选中的 dataId 匹配，则将其标记为选中
        menu.AddItem(new GUIContent(menuPath), _curDataId == dataId, OnDataIdSelected, dataId);
    }

    // 选中一个 dataId
    private void OnDataIdSelected(object dataId)
    {
        _curDataId = (int)dataId;
        _originData = _configMonster.originData.Find(x => x.dataId == _curDataId);
    }

    void AddMenuItemForBuff(GenericMenu menu, string menuPath, int buffDataId)
    {
        // 如果 item 项与当前选中的 dataId 匹配，则将其标记为选中
        menu.AddItem(new GUIContent(menuPath), false, OnBuffSelected, buffDataId);
    }

    private void OnBuffSelected(object buffDataId)
    {
        if (!_originData.carringBuff.Contains((int)buffDataId))
        {
            _originData.carringBuff.Add((int)buffDataId);
        }
    }
}
