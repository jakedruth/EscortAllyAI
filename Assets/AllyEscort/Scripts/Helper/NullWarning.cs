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
        {
            additionalInfo = null;
        }

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
                string message = string.IsNullOrEmpty(warning.additionalInfo)
                    ? "This field cannot be null."
                    : warning.additionalInfo;

                if (property.isArray)
                {
                    if (property.arraySize == 0)
                    {
                        DisplayWarning(property.displayName, message);
                    }
                }
                else if (property.objectReferenceValue == null)
                {
                    DisplayWarning(property.displayName, message);
                }
            }
        }

        public void DisplayWarning(string displayName, string message)
        {
            try
            {
                string combined = $"Warning for field {displayName}: {message}";
                EditorGUILayout.HelpBox(combined, MessageType.Warning);
            }
            catch (Exception)
            {
                // No clue why, but when you first add the a script with this warning box, it freaks out for some reason.
                // So this is hear to throw and exception and then promptly ignore it. For now?

            }
        }
    }
#endif
}



