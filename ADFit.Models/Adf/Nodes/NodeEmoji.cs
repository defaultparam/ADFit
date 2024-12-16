namespace ADFit.Models.Adf.Nodes
{
    public class NodeEmoji : AdfInlineNode
    {
        public Attributes Attrs { get; set; }
        public NodeEmoji(string shortName) : base(NodeTypeInline.emoji)
        {
            Attrs = new Attributes(shortName);
        }

        public NodeEmoji(string shortName, string id, string text) : base(NodeTypeInline.emoji)
        {
            Attrs = new Attributes(shortName, id, text);
        }

        public class Attributes
        {
            public string ShortName { get; }
            public string? Id { get; }
            public string? Text { get; }

            public Attributes(string shortName)
            {
                if (string.IsNullOrWhiteSpace(shortName))
                {
                    throw new ArgumentException("NodeEmoji: ShortName cannot be null or empty");
                }
                ShortName = shortName;
            }

            public Attributes(string shortName, string? id, string? text) : this(shortName)
            {
                Id = id;
                Text = text;
            }
        }
    }
}
