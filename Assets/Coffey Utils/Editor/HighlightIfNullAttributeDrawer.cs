#if UNITY_EDITOR
using UnityEditor;

namespace Coffey_Utils.Editor
{
    [CustomPropertyDrawer(typeof(HighlightIfNullAttribute))]
    public class HighlightIfNullAttributeDrawer : HighlightableAttributeDrawer
    {
        protected override bool ShouldHighlight(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null;
        }
    }
}
#endif