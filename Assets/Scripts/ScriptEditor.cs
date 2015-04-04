using UnityEngine;
using UnityEditor;
using System.Collections;

public class ScriptEditor : EditorWindow
{
    private string script = "";

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Script Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ScriptEditor window = (ScriptEditor)EditorWindow.GetWindow(typeof(ScriptEditor));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.TextArea(script, GUILayout.Height(position.height - 30));
    }
}
