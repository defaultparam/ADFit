namespace ADFit.Models.Adf.Nodes
{
    public class NodeHardBreak : AdfInlineNode
    {
        public Attributes? Attrs { get; set; }
        public NodeHardBreak() : base(NodeTypeInline.hardBreak) { }
        public class Attributes
        {
            public string Text { get; } = "\n";
            public Attributes() { }
        }
    }
}
