namespace ADFit.Models.Adf.Nodes
{
    public class NodeStatus : AdfInlineNode
    {
        public Attributes Attrs { get; set; }

        public NodeStatus(string text, StatusColor color) : base(NodeTypeInline.status)
        {
            Attrs = new Attributes(text, color);
        }

        public class Attributes
        {
            public string Text { get; }
            public StatusColor Color { get; }
            public string LocalId { get; }

            public Attributes(string text, StatusColor color)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new ArgumentException("NodeStatus: text cannot be null or empty");
                }
                Text = text;
                Color = color;
                LocalId = Guid.NewGuid().ToString();
            }
        }

        public enum StatusColor
        {
            neutral,
            purple,
            blue,
            red,
            yellow,
            green
        }
    }
}
