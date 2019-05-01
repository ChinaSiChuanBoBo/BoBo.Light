using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        string valueStr = "";

        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                {
                    valueStr = property.intValue.ToString();
                } break;

            case SerializedPropertyType.Boolean:
                {
                    valueStr = property.boolValue.ToString();
                } break;

            case SerializedPropertyType.Float:
                {
                    valueStr = property.floatValue.ToString("0.00000");
                } break;

            case SerializedPropertyType.String:
                {
                    valueStr = property.stringValue;
                } break;
            case SerializedPropertyType.Enum:
                {
                    valueStr = property.enumNames[property.enumValueIndex];
                } break;

            default: break;
        }
        if (!string.IsNullOrEmpty(valueStr))
            EditorGUI.LabelField(position, label.text, valueStr);
        else
            EditorGUI.PropertyField(position, property);
    }
}

