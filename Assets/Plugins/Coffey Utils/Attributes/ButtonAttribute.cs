using System;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ButtonAttribute : Attribute
{
    public string Label { get; set; } = "";
    public ButtonMode Mode { get; set; } = ButtonMode.Always;
    public int Spacing { get; set; } = 0;
    public ColorField Color { get; set; } = ColorField.None;
}

public enum ButtonMode
{
    Always,
    InPlayMode,
    NotInPlayMode
}