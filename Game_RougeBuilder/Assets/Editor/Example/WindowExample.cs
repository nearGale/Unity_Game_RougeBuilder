using UnityEditor;
using UnityEngine;

public class WindowExample : EditorWindow
{
    private static WindowExample window;//窗体实例

    //显示窗体
    [MenuItem("Tools/Example/Window")]
    private static void ShowWindow()
    {
        window = EditorWindow.GetWindow<WindowExample>("Window Example");
        window.Show();
    }

    //显示时调用
    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    //绘制窗体内容
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Your Second Window", EditorStyles.boldLabel);
    }

    //固定帧数调用
    private void Update()
    {
        Debug.Log("Update");
    }

    //隐藏时调用
    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    //销毁时调用
    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }
}
