namespace ADFit.Models.Adf.Nodes
{
    public class NodeCodeBlock : AdfTopNode
    {
        public Attributes? Attrs { get; set; }

        private List<NodeText> _content = [];
        public List<NodeText> Content
        {
            get => _content;
            set
            {
                ValidateContent(value);
                _content = value;
            }
        }

        public NodeCodeBlock(List<NodeText> content) : base(NodeTypeTop.codeBlock)
        {
            Content = content;
        }

        public NodeCodeBlock(List<NodeText> content, string language) : this(content)
        {
            Attrs = new Attributes(language);
        }

        public class Attributes
        {
            public string Language { get; }

            public Attributes(string language)
            {
                if (string.IsNullOrWhiteSpace(language))
                {
                    throw new ArgumentException("NodeCodeBlock: Language must not be empty");
                }
                Language = language;
            }
        }

        private void ValidateContent(List<NodeText> content)
        {
            if (content is null || content.Count == 0)
            {
                throw new ArgumentException("NodeCodeBlock: Content must contain 1 or more elements");
            }

            bool flag = content.All(x => x.Marks == null);

            if (!flag)
            {
                throw new ArgumentException("NodeCodeBlock: NodeText must not contain any marks");
            }
        }

    }
}
