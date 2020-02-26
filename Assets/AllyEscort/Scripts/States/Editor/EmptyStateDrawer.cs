using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllyEscort
{
    [CustomEditor(typeof(EmptyState))]
    public class EmptyStateDrawer : Editor
    {
        public EmptyState EmptyStateTarget { get; private set; }

        public void OnEnable()
        {
            EmptyStateTarget = (EmptyState) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (GUILayout.Button("Generate Custom State"))
            {
                //Debug.Log(Selection.activeObject.name);
                Object selectedObject = Selection.activeObject;
                string path = AssetDatabase.GetAssetPath(selectedObject);
                string text = File.ReadAllText(path);

                string newFilePath = EditorUtility.SaveFilePanel("Create Behavior Script",
                    path, 
                    Path.GetFileNameWithoutExtension(path),
                    "cs");

                string fileName = Path.GetFileNameWithoutExtension(newFilePath);

                if (newFilePath != null)
                {
                    File.WriteAllText(newFilePath,
                        $@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{{
    public class {fileName} : State
    {{
        internal override void HandleInitialize()
        {{ }}

        internal override void HandleOnEnter()
        {{ }}

        internal override void HandleUpdate()
        {{ }}

        internal override void HandleOnExit()
        {{ }}
    }}
}}");
                    
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    string relativePath = newFilePath.Substring(newFilePath.IndexOf("Assets"));
                    Debug.Log(relativePath);

                    Object newFile = AssetDatabase.LoadAssetAtPath<Object>(relativePath);
                    Debug.Log(newFile.GetType());

                    EditorGUIUtility.PingObject(newFile);

                    EditorUtility.FocusProjectWindow();
                }

                // TODO:
                // 1) Create Template for ScriptableObject of State
                // 2) create file
                // 3) get new file's GUID
                // 4) Switch selectedObject's file with new file
                // 5) ???
                // 6) Profit

            }
        }
    }
}
