#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfAttributeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!ShouldShow(property)) return 0;
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    private bool ShouldShow(SerializedProperty property)
    {
        if (attribute is ShowIfAttribute attr)
        {
            var target = property.serializedObject.targetObject;
            return ShowIfEditorHelper.ShouldShow(target, attr.Targets);
        }
        return true;
    }

}
#endif