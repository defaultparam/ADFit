namespace ADFit.Models.Adf.Nodes
{
    public class NodeInlineCard : AdfInlineNode
    {
        public Attributes Attrs { get; set; }
        public NodeInlineCard(string? url, string? data) : base(NodeTypeInline.inlineCard)
        {
            Attrs = new Attributes(url, data);
        }

        public class Attributes
        {
            public string? Data { get; }
            public string? Url { get; }
            public Attributes(string? url = null, string? data = null)
            {
                if (data != null && url != null)
                {
                    throw new ArgumentException("NodeInlineCard: Only one of the data or url can be set");
                }
                if (data == null && url == null)
                {
                    throw new ArgumentException("NodeInlineCard: One of the data or url must be set");
                }

                Url = url;
                Data = data;
            }
        }
    }
}
