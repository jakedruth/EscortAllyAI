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

                // TODO:
                // 1) Create Template for ScriptableObject of BaseState
                // 2) create file
                // 3) get new file's GUID
                // 4) Switch selectedObject's file with new file
                // 5) ???
                // 6) Profit

            }
        }
    }
}
