using UnityEngine;

public class ShowIfAttribute : PropertyAttribute
{
    public readonly string[] Targets;

    public ShowIfAttribute(params string[] target)
    {
        Targets = target;
    }
}