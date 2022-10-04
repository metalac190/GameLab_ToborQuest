#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SerializedScene))]
public class SerializedSceneDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var validProp = property.FindPropertyRelative("Valid");
        
        var c = GUI.backgroundColor;
        if (!validProp.boolValue) GUI.backgroundColor = Color.red;
        
        var sceneProp = property.FindPropertyRelative("Scene");
        position.height -= 2;
        EditorGUI.ObjectField(position, sceneProp, typeof(SceneAsset), label);
        
        GUI.backgroundColor = c;
    }
}
#endif