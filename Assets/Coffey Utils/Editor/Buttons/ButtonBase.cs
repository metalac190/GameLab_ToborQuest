#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ButtonAttributeEditor
{
    public abstract class ButtonBase
    {
        protected readonly MethodInfo Method;
        protected readonly string DisplayName;
        private readonly bool _disabled;
        private readonly int _spacing;
        private readonly bool _shouldColor;
        private readonly Color _color;

        internal static ButtonBase Create(MethodInfo method, ButtonAttribute buttonAttribute)
        {
            var parameters = method.GetParameters();

            if (parameters.Length == 0)
            {
                return new ButtonWithoutParameters(method, buttonAttribute);
            }
            return new ButtonWithParameters(method, buttonAttribute, parameters);
        }

        protected ButtonBase(MethodInfo method, ButtonAttribute buttonAttribute)
        {
            Method = method;
            DisplayName = string.IsNullOrEmpty(buttonAttribute.Label) ? ObjectNames.NicifyVariableName(method.Name) : buttonAttribute.Label;

            _spacing = buttonAttribute.Spacing;
            _disabled = buttonAttribute.Mode switch
            {
                ButtonMode.Always => false,
                ButtonMode.InPlayMode => !EditorApplication.isPlaying,
                ButtonMode.NotInPlayMode => EditorApplication.isPlaying,
                _ => true
            };
            _shouldColor = buttonAttribute.Color != ColorField.None;
            if (_shouldColor)
            {
                _color = HighlightableAttribute.ConvertColor(buttonAttribute.Color);
            }
        }

        public void Draw(IEnumerable<object> targets)
        {
            if (_spacing > 0) GUILayout.Space(_spacing);
            
            EditorGUI.BeginDisabledGroup(_disabled);
            
            var backgroundColor = GUI.backgroundColor;
            if (_shouldColor) GUI.backgroundColor = _color;
            
            DrawInternal(targets);
            
            GUI.backgroundColor = backgroundColor;
            
            EditorGUI.EndDisabledGroup();
        }

        protected abstract void DrawInternal(IEnumerable<object> targets);
    }
}
#endif