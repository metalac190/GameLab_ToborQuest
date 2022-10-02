using System;
using UnityEngine;

[Serializable]
public struct RangedFloat
{
    // Please add [MinMaxRange(float min, float max)] to establish a custom range
    // Otherwise this will assume a min of 0 and a max of 1.
    
    public float MinValue;
    public float MaxValue;

    public RangedFloat(float value) {
        MinValue = value;
        MaxValue = value;
    }

    public RangedFloat(float min, float max) {
        MinValue = min;
        MaxValue = max;
    }

    public float GetRandom() {
        return UnityEngine.Random.Range(MinValue, MaxValue);
    }

    public float Clamp(float value) {
        return Mathf.Clamp(value, MinValue, MaxValue);
    }
}