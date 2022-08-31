﻿using System;
using UnityEditor;
using UnityEngine;

namespace ButtonAttributeEditor.Utils
{
    [Serializable]
    internal class NonSerializedError
    {
    }

    [CustomPropertyDrawer(typeof(NonSerializedError))]
    internal class NonSerializedErrorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            Rect rectWithoutLabel = EditorGUI.PrefixLabel(rect, label);

            EditorGUI.HelpBox(rectWithoutLabel, "Unable to draw a non-serialized type.", MessageType.Error);
        }
    }
}