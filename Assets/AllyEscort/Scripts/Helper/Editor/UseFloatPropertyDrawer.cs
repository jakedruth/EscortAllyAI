using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UseFloat))]
[CustomPropertyDrawer(typeof(ValueName))]
public class UseFloatPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rect
        const float checkBoxWidth = 17f;
        Rect useRect = new Rect(position.x, position.y, checkBoxWidth, position.height);

        // Get the serialized properties
        SerializedProperty useProperty = property.FindPropertyRelative("use");
        SerializedProperty valueProperty = property.FindPropertyRelative("value");

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(useRect, useProperty, GUIContent.none);
        if (useProperty.boolValue)
        {
            // determine the title of value
            string title = !(attribute is ValueName a) ? "Value:" : a.label;

            // calculate the width of the title
            const float minValueRectWidth = 63f; // hard-coded rough-estimate minimum width
            float maxTitleWidth = position.width - minValueRectWidth;
            float titleWidth = Mathf.Min(maxTitleWidth, GUI.skin.label.CalcSize(new GUIContent(title)).x);
            float offset = checkBoxWidth + titleWidth;

            // calculate rects
            Rect valueLabelRect = new Rect(position.x + checkBoxWidth, position.y, titleWidth, position.height);
            Rect valueRect = new Rect(position.x + offset, position.y, position.width - offset, position.height);

            // draw the labels
            EditorGUI.LabelField(valueLabelRect, title, GUIStyle.none);
            EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}