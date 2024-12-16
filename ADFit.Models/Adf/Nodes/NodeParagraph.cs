namespace ADFit.Models.Adf.Nodes
{
    public class NodeParagraph : AdfTopNode
    {
        public Attributes? Attrs { get; set; }
        public List<AdfInlineNode> Content { get; set; }

        public NodeParagraph(List<AdfInlineNode> content) : base(NodeTypeTop.paragraph)
        {
            Content = content;
        }
        public NodeParagraph(List<AdfInlineNode> content, string? localId) : this(content)
        {
            Attrs = new Attributes(localId);
        }

        public class Attributes
        {
            public string? LocalId { get; }
            public Attributes(string? localId)
            {
                LocalId = localId;
            }
        }
    }
}
