using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateStateEditor
{
    [MenuItem("Assets/Create/Ally Escort", menuItem = "Assets/Create/Ally Escort/States/Create New State", priority = 1)]
    public static void Create()
    {
        string[] selection = Selection.assetGUIDs;

        if (selection.Length == 0)
        {
            return;
        }

        string selectionPath = AssetDatabase.GUIDToAssetPath(selection[0]);
        Debug.Log(selectionPath);

        string test = EditorUtility.SaveFilePanel("Create New State", selectionPath, "NewState", "cs");
        Debug.Log(test);
    }
}
