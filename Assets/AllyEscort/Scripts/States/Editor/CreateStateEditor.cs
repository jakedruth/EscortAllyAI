using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CreateStateEditor
{
    [MenuItem("Assets/Create/Ally Escort", menuItem = "Assets/Create/Ally Escort/States/Create New State", priority = 1)]
    public static void Create()
    {
        ScriptableObject asset = ScriptableObject.CreateInstance<BaseState>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == null)
            return;

        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New State.asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
