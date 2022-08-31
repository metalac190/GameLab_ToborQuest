using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ShowIfAttribute : PropertyAttribute
{
    public readonly string[] Targets;

    public ShowIfAttribute(params string[] target)
    {
        Targets = target;
    }
}