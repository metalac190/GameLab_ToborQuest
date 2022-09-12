using UnityEngine;

public class MinMaxRangeAttribute : PropertyAttribute
{
    public float Min { get; }
    public float Max { get; }

    public MinMaxRangeAttribute(float min, float max) {
        Min = min;
        Max = max;
    }
}