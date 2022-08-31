#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(HighlightAttribute))]
public class HighlightAttributeDrawer : HighlightableAttributeDrawer
{
    protected override bool ShouldHighlight(SerializedProperty property) => true;
}
#endif