#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LevelMetals))]
public class LevelMetalsDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    {
        var bronzeProp = property.FindPropertyRelative("Bronze");
        var silverProp = property.FindPropertyRelative("Silver");
        var goldProp = property.FindPropertyRelative("Gold");
        var platinumProp = property.FindPropertyRelative("Platinum");

        var spacing = 4;
        var textWidth = 16;
        var quarter = pos.width * 0.25f;
        var textRect = new Rect(pos.x, pos.y, textWidth, pos.height);
        var floatRect = new Rect(pos.x + textWidth, pos.y, quarter - textWidth - spacing, pos.height - 2);

        GUI.Label(textRect, "P");
        EditorGUI.PropertyField(floatRect, platinumProp, GUIContent.none);
        textRect.x += quarter;
        floatRect.x += quarter;
        GUI.Label(textRect, "G");
        EditorGUI.PropertyField(floatRect, goldProp, GUIContent.none);
        textRect.x += quarter;
        floatRect.x += quarter;
        GUI.Label(textRect, "S");
        EditorGUI.PropertyField(floatRect, silverProp,  GUIContent.none);
        textRect.x += quarter;
        floatRect.x += quarter;
        GUI.Label(textRect, "B");
        EditorGUI.PropertyField(floatRect, bronzeProp, GUIContent.none);
    }
}
#endif