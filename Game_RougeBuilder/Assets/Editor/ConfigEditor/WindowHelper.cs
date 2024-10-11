using UnityEditor;

public static class WindowHelper
{
    public static void IntField(string title, ref int val)
    {
        var str = EditorGUILayout.TextField($"{title}: ", val.ToString());
        val = int.Parse(str);
    }

    public static void StrField(string title, ref string val)
    {
        var str = EditorGUILayout.TextField($"{title}: ", val);
        val = str;
    }

    public static void FloatField(string title, ref float val)
    {
        var str = EditorGUILayout.TextField($"{title}: ", val.ToString());
        val = float.Parse(str);
    }
}