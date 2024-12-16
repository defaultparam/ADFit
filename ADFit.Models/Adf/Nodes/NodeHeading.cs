namespace ADFit.Models.Adf.Nodes
{
    public class NodeHeading : AdfTopNode
    {
        public Attributes Attrs { get; set; }
        public List<AdfInlineNode> Content { get; set; } = [];
        public NodeHeading(int level) : base(NodeTypeTop.heading)
        {
            Attrs = new Attributes(level);
        }

        public NodeHeading(int level, List<AdfInlineNode> content, string? localId = null) : base(NodeTypeTop.heading)
        {
            Content = content;
            Attrs = new Attributes(level, localId);
        }

        public class Attributes
        {
            public int Level { get; }
            public string? LocalId { get; }

            public Attributes(int level)
            {
                if (level < 1 || level > 6)
                {
                    throw new ArgumentOutOfRangeException(nameof(level), "NodeHeading: heading level must be between 1 and 6.");
                }
                Level = level;
            }

            public Attributes(int level, string? localId) : this(level)
            {
                LocalId = localId;
            }
        }
    }
}
