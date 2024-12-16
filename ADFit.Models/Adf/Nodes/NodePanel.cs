
namespace ADFit.Models.Adf.Nodes
{
    public class NodePanel : AdfTopNode
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

        public NodePanel(PanelType panelType, List<AdfTopNode> content) : base(NodeTypeTop.panel)
        {
            Content = content;
            Attrs = new Attributes(panelType);
        }

        public class Attributes
        {
            public PanelType Type { get; }

            public Attributes(PanelType type)
            {
                Type = type;
            }
        }

        private void ValidateContent(List<AdfTopNode> content)
        {
            if (content is null || content.Count == 0)
            {
                throw new ArgumentException("NodePanel: Content must contain 1 or more elements");
            }

            bool flag = content.All(content => content.Type == NodeTypeTop.paragraph
            || content.Type == NodeTypeTop.heading
            || content.Type == NodeTypeTop.bulletList
            || content.Type == NodeTypeTop.orderedList);

            if (!flag)
            {
                throw new ArgumentException("NodePanel: Content can only child the following nodes: NodeParagraph, NodeHeading, NodeBulletList and NodeOrderedList");
            }
        }

        public enum PanelType
        {
            info,
            note,
            warning,
            success,
            error
        }
    }
}
