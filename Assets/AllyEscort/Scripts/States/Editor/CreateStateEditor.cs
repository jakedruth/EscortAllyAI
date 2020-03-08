using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace AllyEscort
{
    public class CreateStateEditor
    {
        private static double _renameTime;

        [MenuItem("Assets/Create", menuItem = "Assets/Create/Ally Escort/Create New State",
            priority = 1)]
        public static void Create()
        {
            ScriptableObject asset = ScriptableObject.CreateInstance<EmptyState>();

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

            EditorApplication.update += EngageRenameMode;

            _renameTime = EditorApplication.timeSinceStartup + 0.4d;
            //EditorApplication.ExecuteMenuItem("UnityEditor.ObjectBrowser");

            
        }

        private static void EngageRenameMode()
        {
            if (EditorApplication.timeSinceStartup >= _renameTime)
            {
                EditorApplication.update -= EngageRenameMode;

                Assembly assembly = typeof(EditorWindow).Assembly;

                Type type = assembly.GetType("UnityEditor.ProjectBrowser");
                EditorWindow projectWindow = EditorWindow.GetWindow(type);

                if (projectWindow != null)
                {
                    projectWindow.SendEvent(new Event { keyCode = KeyCode.F2, type = EventType.KeyDown });
                }
            }
        }
    }
}
