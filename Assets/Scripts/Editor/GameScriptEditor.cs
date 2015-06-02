using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text.RegularExpressions;

[CustomEditor(typeof(GameScript))]
public class GameScriptEditor : Editor
{
    private GameScript _target;
    private string _path;

    [MenuItem("Assets/Create/Winston/Game Script")]
    public static GameScript CreateAsset()
    {
        GameScript asset = CreateInstance<GameScript>();
        ProjectWindowUtil.CreateAsset(asset, "New Game Script" + ".asset");
        return asset;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Parse"))
        {
            _path = EditorUtility.OpenFolderPanel("Select Folder", "", "What");
            if (_path.Length != 0)
            {
                // Folder selected.
            }
        }
    }
}