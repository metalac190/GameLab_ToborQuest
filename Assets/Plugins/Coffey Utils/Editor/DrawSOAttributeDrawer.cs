#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Coffey_Utils.Editor
{
    [CustomPropertyDrawer(typeof(DrawSOAttribute), true)]
    public class DrawSOAttributeDrawer : PropertyDrawer
    {
        private const bool ShowDisabled = true;
        private const float Padding = 5f;

        private UnityEditor.Editor _editor;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0f;

            totalHeight += EditorGUIUtility.singleLineHeight;

            if (property.objectReferenceValue == null || !property.isExpanded)
                return totalHeight;

            if (_editor == null)
                UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref _editor);

            if (_editor == null)
                return totalHeight;

            SerializedProperty field = _editor.serializedObject.GetIterator();
            field.NextVisible(true);
            while (field.NextVisible(false))
            {
                totalHeight += EditorGUI.GetPropertyHeight(field, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            totalHeight += Padding * 4;

            return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect fieldRect = new Rect(position)
            {
                height = EditorGUIUtility.singleLineHeight
            };

            EditorGUI.PropertyField(fieldRect, property, label, true);

            if (property.objectReferenceValue == null)
                return;

            property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, GUIContent.none, true);

            if (!property.isExpanded)
                return;
        
            EditorGUI.BeginDisabledGroup(ShowDisabled);

            if (_editor == null)
                UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref _editor);

            if (_editor == null)
                return;

            var propertyRects = new List<Rect>();
            Rect marchingRect = new Rect(fieldRect);
            marchingRect.width -= Padding;

            Rect bodyRect = new Rect(fieldRect);
            bodyRect.xMin += EditorGUI.indentLevel * 18;
            bodyRect.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty field = _editor.serializedObject.GetIterator();
            field.NextVisible(true);

            marchingRect.y += Padding;

            while (field.NextVisible(false))
            {
                marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;
                marchingRect.height = EditorGUI.GetPropertyHeight(field, true);
                propertyRects.Add(marchingRect);
            }

            marchingRect.y += Padding;

            bodyRect.yMax = marchingRect.yMax;

            EditorGUI.HelpBox(bodyRect, "", MessageType.None);

            EditorGUI.indentLevel++;

            int index = 0;
            field = _editor.serializedObject.GetIterator();
            field.NextVisible(true);

            while (field.NextVisible(false))
            {
                try
                {
                    EditorGUI.PropertyField(propertyRects[index], field, true);
                }
                catch (StackOverflowException)
                {
                    field.objectReferenceValue = null;
                    Debug.LogError("Nesting Error on Scriptable Object Editor");
                }
                index++;
            }
            EditorGUI.indentLevel--;
        
            EditorGUI.EndDisabledGroup();
        }
    }
}
/*
[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class MonoBehaviourEditor : Editor { }

[CanEditMultipleObjects]
[CustomEditor(typeof(ScriptableObject), true)]
public class ScriptableObjectEditor : Editor { }
*/
#endif