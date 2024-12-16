
namespace ADFit.Models.Adf.Nodes
{
    public class NodeNestedExpand : AdfChildNode
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

        public NodeNestedExpand(string title, List<AdfTopNode> content) : base(NodeTypeChild.nestedExpand)
        {
            Attrs = new Attributes(title);
            Content = content;
        }

        public class Attributes
        {
            public string Title { get; }

            public Attributes(string title)
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    throw new ArgumentException("nestedExpand: Title must not be empty");
                }
                Title = title;
            }
        }

        private void ValidateContent(List<AdfTopNode> content)
        {
            if (content is null || content.Count == 0)
            {
                throw new ArgumentException("nestedExpand: Content must have 1 or more elements");
            }
            bool flag = content.All(x => x.Type == NodeTypeTop.paragraph
                || x.Type == NodeTypeTop.heading
                || x.Type == NodeTypeTop.mediaGroup
                || x.Type == NodeTypeTop.mediaSingle);

            if (!flag)
            {
                throw new ArgumentException("nestedExpand: Content can only child the following nodes: NodeHeading, NodeParagraph, NodeMediaGroup or NodeMediaSingle");
            }
        }

    }
}
