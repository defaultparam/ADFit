namespace ADFit.Models.Adf.Nodes
{
    public class NodeExpand : AdfTopNode
    {
        public Attributes Attrs { get; set; }
        private List<AdfTopNode> _content = [];
        public List<AdfTopNode> Content
        {
            get => _content;
            set
            {
                ValidateContent(value);
                _content = value;
            }
        }

        public NodeExpand(List<AdfTopNode> content) : base(NodeTypeTop.expand)
        {
            Content = content;
            Attrs = new Attributes();
        }

        public NodeExpand(string title, List<AdfTopNode> content) : base(NodeTypeTop.expand)
        {
            Attrs = new Attributes(title);
            Content = content;
        }

        public class Attributes
        {
            public string? Title { get; }

            public Attributes() { }

            public Attributes(string title)
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentException("NodeExpand: Title must not be empty");
                }
                Title = title;
            }
        }

        private void ValidateContent(List<AdfTopNode> content)
        {
            if (content.Count == 0)
            {
                throw new ArgumentException("NodeExpand: Content must contain 1 or more elements");
            }
        }
    }
}
