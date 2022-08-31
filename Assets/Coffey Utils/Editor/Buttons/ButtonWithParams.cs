#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ButtonAttributeEditor.Utils;
using UnityEditor;
using UnityEngine;

namespace ButtonAttributeEditor
{
    using Object = UnityEngine.Object;

    internal class ButtonWithParameters : ButtonBase
    {
        private readonly Parameter[] _parameters;
        private bool _expanded;

        public ButtonWithParameters(MethodInfo method, ButtonAttribute buttonAttribute, IEnumerable<ParameterInfo> parameters)
            : base(method, buttonAttribute) {
            _parameters = parameters.Select(paramInfo => new Parameter(paramInfo)).ToArray();
            _expanded = true;
        }

        // TODO: Draw default values for parameters

        protected override void DrawInternal(IEnumerable<object> targets) {
            Rect foldoutRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.foldoutHeader);
            _expanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, _expanded, DisplayName);
            if (_expanded) {
                EditorGUI.indentLevel++;
                foreach (Parameter param in _parameters) {
                    param.Draw();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (_expanded) {
                if (GUILayout.Button(DisplayName)) {
                    foreach (object obj in targets) {
                        Method.Invoke(obj, _parameters.Select(param => param.Value).ToArray());
                    }
                }
            }
        }

        private readonly struct Parameter
        {
            private readonly FieldInfo _fieldInfo;
            private readonly ScriptableObject _scriptableObj;
            private readonly NoScriptFieldEditor _editor;

            public Parameter(ParameterInfo paramInfo) {
                Type generatedType = ScriptableObjectCache.GetClass(paramInfo.Name, paramInfo.ParameterType);
                _scriptableObj = ScriptableObject.CreateInstance(generatedType);
                _fieldInfo = generatedType.GetField(paramInfo.Name);
                _editor = CreateEditor<NoScriptFieldEditor>(_scriptableObj);
            }

            public object Value {
                get {
                    _editor.ApplyModifiedProperties();
                    return _fieldInfo.GetValue(_scriptableObj);
                }
            }

            public void Draw() {
                _editor.OnInspectorGUI();
            }

            private static TEditor CreateEditor<TEditor>(Object obj)
                where TEditor : Editor {
                return (TEditor)Editor.CreateEditor(obj, typeof(TEditor));
            }
        }
    }
}
#endif