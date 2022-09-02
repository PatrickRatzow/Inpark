using System.Text;

namespace Zeta.Inpark.Common.SDUI;

// ReSharper disable once InconsistentNaming
public class SDUINode
{
    private Dictionary<string, object> InternalData { get; }
    public string? InnerText { get; private set; }
    public string Name { get; }
    public Dictionary<string, string> Attributes { get; }
    internal List<SDUINode> Children { get; }
    
    public SDUINode(
        string name,
        string? innerText = null,
        Dictionary<string, string>? attributes = null,
        List<SDUINode>? children = null)
    {
        InnerText = innerText;
        Name = name;
        Attributes = attributes ?? new Dictionary<string, string>();
        Children = children ?? new List<SDUINode>();
        InternalData = new();
    }

    public virtual void AddChild(SDUINode child)
    {
        Children.Add(child);
    }
    
    public void SetAttribute(string key, string value)
    {
        Attributes[key] = value;
    }

    public void SetText(string text)
    {
        InnerText = text;
    }

    public void SetDebug(string text)
    {
        SetAttribute("z-debug", text);
    }
    
    public void SetParent(SDUINode parent)
    {
        parent.AddChild(this);
    }

    public void ShouldNotTranslate()
    {
        if (!Attributes.TryGetValue("class", out var classString))
        {
            classString = string.Empty;
        }

        var classSplit = classString.Trim().Split(" ").ToList();
        classSplit.Add("notranslate");

        SetAttribute("class", string.Join(" ", classSplit));
    }
    
    internal void SetInternalData<T>(string key, string internalKey, T data) 
        => InternalData[key] = new InternalData<T>(internalKey, data);
    internal InternalData<T>? GetInternalData<T>(string key) 
        => InternalData.TryGetValue(key, out var data) ? (InternalData<T>)data : default;
}

public record InternalData<T>(
    string Key,
    T Value
);