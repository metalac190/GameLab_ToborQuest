#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ButtonAttributeEditor
{
    internal class ButtonWithoutParameters : ButtonBase
    {
        public ButtonWithoutParameters(MethodInfo method, ButtonAttribute buttonAttribute) : base(method, buttonAttribute) { }

        protected override void DrawInternal(IEnumerable<object> targets) {
            if (GUILayout.Button(DisplayName)) {
                foreach (object obj in targets) {
                    Method.Invoke(obj, null);
                }
            }
        }
    }
}
#endif