// Automatically change the name of the selected object via a text field
using UnityEditor;
using UnityEngine;

public class EditorGUILayoutTextField : EditorWindow
{
    [MenuItem("Tools/Example/GUILayout TextField")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorGUILayoutTextField));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Select an object in the hierarchy view");
        if (Selection.activeGameObject)
            Selection.activeGameObject.name =
                EditorGUILayout.TextField("Object Name: ", Selection.activeGameObject.name);
        this.Repaint();
    }
}