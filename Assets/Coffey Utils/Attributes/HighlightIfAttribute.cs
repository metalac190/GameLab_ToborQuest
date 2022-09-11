using System.Drawing;

public class HighlightIfAttribute : HighlightableAttribute
{
    public readonly string[] Targets;

    public HighlightIfAttribute(params string[] targets) : base(ColorField.Green) { Targets = targets; }
    public HighlightIfAttribute(ColorField color, params string[] targets) : base(color) { Targets = targets; }
    public HighlightIfAttribute(ColorField color, HighlightMode mode = HighlightMode.Back, params string[] targets) : base(color, mode) { Targets = targets; }
    public HighlightIfAttribute(float r, float g, float b, params string[] targets) : base(r, g, b) { Targets = targets; }
    public HighlightIfAttribute(float r, float g, float b, HighlightMode mode = HighlightMode.Back, params string[] targets) : base(r, g, b, mode) { Targets = targets; }
    public HighlightIfAttribute(int r, int g, int b, params string[] targets) : base(r, g, b) { Targets = targets; }
    public HighlightIfAttribute(int r, int g, int b, HighlightMode mode = HighlightMode.Back, params string[] targets) : base(r, g, b, mode) { Targets = targets; }
    public HighlightIfAttribute(KnownColor color, params string[] targets) : base(color) { Targets = targets; }
    public HighlightIfAttribute(KnownColor color, HighlightMode mode = HighlightMode.Back, params string[] targets) : base(color, mode) { Targets = targets; }
}