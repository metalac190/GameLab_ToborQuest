using System;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ReadOnlyAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : PropertyDrawer
{
    // TODO: Also hide / disable the SIZE of arrays and list in inspector

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif