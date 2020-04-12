using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AllyEscort
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class NullWarning : PropertyAttribute
    {
        public string additionalInfo;

        public NullWarning()
        { }

        public NullWarning(string info)
        {
            additionalInfo = info;
        }
    }


#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(NullWarning))]
    public class NullInspectorWarningAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);

            if (attribute is NullWarning warning)
            {
                if (property.isArray)
                {
                    if (property.arraySize == 0)
                    {
                        string message = $"Warning: The field {property.displayName} can not be empty.\n{warning.additionalInfo}";
                        DisplayWarning(message);
                    }
                }
                else if (property.objectReferenceValue == null)
                {
                    string message = $"Warning: The field {property.displayName} can not be null.\n{warning.additionalInfo}";
                    DisplayWarning(message); 
                }
            }
        }

        public void DisplayWarning(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }
    }
#endif
}



