#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;

namespace Coffey_Utils.Editor.Buttons
{
    public class ButtonsDrawer
    {
        private readonly List<ButtonBase> _buttons = new List<ButtonBase>();

        public ButtonsDrawer(object target) {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var methods = target.GetType().GetMethods(flags);

            foreach (var method in methods) {
                ButtonAttribute buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();

                if (buttonAttribute != null) {
                    _buttons.Add(ButtonBase.Create(method, buttonAttribute));
                }
            }
        }

        public void DrawButtons(IEnumerable<object> targets) {
            foreach (ButtonBase button in _buttons) {
                button.Draw(targets);
            }
        }
    }
}
#endif