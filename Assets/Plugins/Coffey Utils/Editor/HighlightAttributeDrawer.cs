#if UNITY_EDITOR
using UnityEditor;

namespace Coffey_Utils.Editor
{
    [CustomPropertyDrawer(typeof(HighlightAttribute))]
    public class HighlightAttributeDrawer : HighlightableAttributeDrawer
    {
        protected override bool ShouldHighlight(SerializedProperty property) => true;
    }
}
#endif