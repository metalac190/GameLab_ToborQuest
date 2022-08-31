#if UNITY_EDITOR
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;

public static class ShowIfEditorHelper
{
    public static bool ShouldShow(object target, string[] booleanFields)
    {
        bool show = true;
        foreach (var field in booleanFields)
        {
            FieldInfo conditionField = GetField(target, field);
            if (conditionField != null && conditionField.FieldType == typeof(bool))
            {
                show &= (bool)conditionField.GetValue(target);
            }
        }
        return show;
    }

    private static FieldInfo GetField(object target, string fieldName)
    {
        return GetAllFields(target, f => f.Name.Equals(fieldName, StringComparison.InvariantCulture)).FirstOrDefault();
    }

    private static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
    {
        var types = new List<Type>
        {
            target.GetType()
        };

        while (types.Last().BaseType != null)
        {
            types.Add(types.Last().BaseType);
        }

        for (int i = types.Count - 1; i >= 0; i--)
        {
            var bind = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
            var fieldInfos = types[i].GetFields(bind).Where(predicate);

            foreach (var fieldInfo in fieldInfos)
            {
                yield return fieldInfo;
            }
        }
    }
}
#endif