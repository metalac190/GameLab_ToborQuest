using System.Drawing;

public class HighlightAttribute : HighlightableAttribute
{
    public HighlightAttribute() : base(ColorField.Green) {}
    public HighlightAttribute(ColorField color, HighlightMode mode = HighlightMode.Back) : base(color, mode) {}
    public HighlightAttribute(float r, float g, float b, HighlightMode mode = HighlightMode.Back) : base(r, g, b, mode) {}
    public HighlightAttribute(int r, int g, int b, HighlightMode mode = HighlightMode.Back) : base(r, g, b, mode) {}
    public HighlightAttribute(KnownColor color, HighlightMode mode = HighlightMode.Back) : base(color, mode) {}
}