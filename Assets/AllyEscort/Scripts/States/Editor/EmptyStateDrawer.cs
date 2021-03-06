﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AllyEscort
{
    [CustomEditor(typeof(EmptyState))]
    public class EmptyStateDrawer : Editor
    {

        public void OnEnable()
        {
            //EmptyStateTarget = (EmptyState) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (GUILayout.Button("Create New State Script"))
            {
                // Get the selected object (the scriptable object that wants a custom state behavior)
                Object selectedObject = Selection.activeObject;
                string path = AssetDatabase.GetAssetPath(selectedObject);
                string text = File.ReadAllText(path);

                // Prompt a save file dialog and get a path to it
                string newFilePath = EditorUtility.SaveFilePanel("Create New State Script", path, 
                    $"{Path.GetFileNameWithoutExtension(path)}State", "cs");
                
                // null if user clicks cancel
                if (newFilePath != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(newFilePath);

                    // check to see if the file contains any 
                    if (fileName.Contains(" "))
                    {
                        throw new ArgumentException($"The file name can not contain any space: {fileName}");
                    }

                    string newFileText = 
// Fancy notation for a interpolated verbatim string literal
$@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{{
    public class {fileName} : State
    {{
        protected override bool HandleInitialize()
        {{
            return true;
        }}

        protected override void HandleOnEnter()
        {{ }}

        protected override void HandleUpdate()
        {{ }}

        protected override void HandleOnExit()
        {{ }}
    }}
}}";
                    // create file from template
                    File.WriteAllText(newFilePath, newFileText);
                    
                    // Used to reload the editor
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    // Load the newly created file from a relative path
                    string relativePath = newFilePath.Substring(newFilePath.IndexOf("Assets", StringComparison.Ordinal));
                    Object newFile = AssetDatabase.LoadAssetAtPath<Object>(relativePath);

                    // Get the new file's guid and localID
                    if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(newFile, out string guid, out long localId))
                    {
                        // read the selected objects text to get the element "m_Script"
                        string scriptElementName = "m_Script";
                        int scriptElementStartIndex = text.IndexOf(scriptElementName, StringComparison.Ordinal);

                        // Extract the fileID
                        int fileIDIndexStart = text.IndexOf("fileID: ", scriptElementStartIndex, StringComparison.Ordinal) + 8; // manually offset index by string length of "fileID: " 
                        int fileIDIndexEnd = text.IndexOf(',', fileIDIndexStart);
                        string fileIDCurrent = text.Substring(fileIDIndexStart, fileIDIndexEnd - fileIDIndexStart);

                        // Extract the guid
                        int guidIndexStart = text.IndexOf("guid: ", scriptElementStartIndex, StringComparison.Ordinal) + 6;     // manually offset index by string length of "guid: " 
                        int guidIndexEnd = text.IndexOf(',', guidIndexStart);
                        string guidCurrent = text.Substring(guidIndexStart, guidIndexEnd - guidIndexStart);


                        // Replace the selected objects guid and fileId with the new ones
                        text = text.Replace(fileIDCurrent, localId.ToString());
                        text = text.Replace(guidCurrent, guid);

                        // ReWrite the new text of the scriptable object to the path
                        File.WriteAllText(path, text);

                        // Reload the editor
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        Selection.activeObject = selectedObject;
                    }

                    // Attempt to show the new object in the project tab
                    EditorGUIUtility.PingObject(newFile);
                    EditorUtility.FocusProjectWindow();

                    AssetDatabase.OpenAsset(newFile);
                }
            }
        }
    }
}
