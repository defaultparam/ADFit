namespace ADFit.Models.Adf.Nodes
{
    public class NodeTableHeader : AdfChildNode
    {
        public Attributes? Attrs { get; set; } = null;

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

        public NodeTableHeader(List<AdfTopNode> content) : base(NodeTypeChild.tableHeader)
        {
            Content = content;
        }

        public NodeTableHeader(List<AdfTopNode> content, string? backgroundColor, List<int>? colwidth, int colspan = 1, int rowspan = 1) : this(content)
        {
            Attrs = new Attributes(backgroundColor, colwidth, colspan, rowspan);
        }

        public class Attributes
        {
            public string? Background { get; }
            public int Colspan { get; }
            public int RowSpan { get; }
            public List<int>? Colwidth { get; }

            public Attributes(string? background = null, List<int>? colwidth = null, int colspan = 1, int rowspan = 1)
            {
                Background = background;
                Colspan = colspan;
                RowSpan = rowspan;
                Colwidth = colwidth;
            }
        }

        private void ValidateContent(List<AdfTopNode> content)
        {
            if (content is null || content.Count == 0)
            {
                throw new ArgumentException("NodeTableHeader: Content must contain 1 or more elements");
            }

            bool flag = content.All(x => x.Type == NodeTypeTop.blockquote
                || x.Type == NodeTypeTop.bulletList
                || x.Type == NodeTypeTop.orderedList
                || x.Type == NodeTypeTop.codeBlock
                || x.Type == NodeTypeTop.mediaGroup
                || x.Type == NodeTypeTop.heading
                //|| x.Type == NodeTypeChild.nestedExpand
                || x.Type == NodeTypeTop.panel
                || x.Type == NodeTypeTop.paragraph
                || x.Type == NodeTypeTop.rule);

            if (!flag)
            {
                throw new ArgumentException("NodeTableHeader: Must be a blockquote, bullet list, ordered list, code block, media group, heading, panel, paragraph, or rule");
            }
        }
    }
}
