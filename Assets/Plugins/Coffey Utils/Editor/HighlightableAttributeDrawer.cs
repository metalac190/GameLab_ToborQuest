#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Coffey_Utils.Editor
{
    [CustomPropertyDrawer(typeof(HighlightableAttribute))]
    public abstract class HighlightableAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!ShouldHighlight(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }
            var contentColor = GUI.contentColor;
            var backgroundColor = GUI.backgroundColor;
            if (attribute is HighlightableAttribute attr)
            {
                var color = attr.Color;
                switch (attr.Mode)
                {
                    case HighlightMode.Back:
                        GUI.backgroundColor = Color.Lerp(backgroundColor, color, 0.5f);
                        color.a = 0.333f;
                        EditorGUI.DrawRect(position, color);
                        break;
                    case HighlightMode.Text:
                        GUI.contentColor = Color.Lerp(contentColor, color, 0.5f);
                        GUI.backgroundColor = Color.Lerp(backgroundColor, color, 0.5f);
                        break;
                }
            }
            EditorGUI.PropertyField(position, property, label, true);
            GUI.contentColor = contentColor;
            GUI.backgroundColor = backgroundColor;
        }

        protected abstract bool ShouldHighlight(SerializedProperty property);
    }
}
#endif