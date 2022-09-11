using System.Drawing;

public class HighlightIfNullAttribute : HighlightableAttribute
{
    public HighlightIfNullAttribute() : base(ColorField.Red) { }
    public HighlightIfNullAttribute(ColorField color, HighlightMode mode = HighlightMode.Back) : base(color, mode) {}
    public HighlightIfNullAttribute(float r, float g, float b, HighlightMode mode = HighlightMode.Back) : base(r, g, b, mode) {}
    public HighlightIfNullAttribute(int r, int g, int b, HighlightMode mode = HighlightMode.Back) : base(r, g, b, mode) {}
    public HighlightIfNullAttribute(KnownColor color, HighlightMode mode = HighlightMode.Back) : base(color, mode) {}
}