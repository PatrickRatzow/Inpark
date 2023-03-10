using Zeta.UI.Intermediate.Nodes;
using Zeta.UI.Transformers.Html;

namespace Zeta.UI.Mappers.Transformers;

public class TextTransformer : ITransformer<TextNode>
{
    private readonly ITransformerService _transformerService;
    
    public TextTransformer(ITransformerService transformerService)
    {
        _transformerService = transformerService;
    }

    public async ValueTask<SDUINode> Transform(TextNode node)
    {
        if (node.Kind is TextKind.Html)
            return await _transformerService.Transform(node.Text);
        
        var text = new Text(node.Text);

        return text;
    }
}