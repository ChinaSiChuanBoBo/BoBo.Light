using System.Reflection;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(ButtonAttribute), true)]
public class ButtonAttributeDrawer : PropertyDrawer
{


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ButtonAttribute attribute = (ButtonAttribute)this.attribute;
        if (!attribute.showOnPlay && Application.isPlaying)
        {
            return -2;
        }
        return attribute.height;
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ButtonAttribute attribute = (ButtonAttribute)this.attribute;

        if (!attribute.showOnPlay &&Application.isPlaying)
        {
            return;
        }

        if (attribute.funcNames == null || attribute.funcNames.Length == 0)
        {
            position = EditorGUI.IndentedRect(position);
            EditorGUI.HelpBox(position, "[Button] funcNames is Not Set!", MessageType.Warning);
            return;
        }

        float width = position.width / attribute.funcNames.Length;
        position.width = width;
        for (int i = 0; i < attribute.funcNames.Length; i++)
        {
            string funcName = attribute.funcNames[i];
            if (GUI.Button(position, funcName))
            {
                CalledFunc(property.serializedObject.targetObject, funcName);
            }
            position.x += width;
        }
    }

    public static void CalledFunc(Object target, string funcName)
    {
        //找脚本上的FR函数，编辑器调用
        MethodInfo methodInfo = target.GetType().GetMethod(funcName, BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo == null)
        {
            return;
        }
        methodInfo.Invoke(target, null);
    }
}
