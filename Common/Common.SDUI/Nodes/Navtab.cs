namespace Zeta.Inpark.Common.SDUI;

public class Navtab : SDUINode
{
    public Navtab(string text, string icon) : base("Navtab")
    {
        var child = new Text(text);
        child.SetParent(this);
        //SetAttribute("text", $"z-translate({text})");
        SetAttribute("icon", icon);
    }
}