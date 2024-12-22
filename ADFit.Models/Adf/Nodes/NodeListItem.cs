
namespace ADFit.Models.Adf.Nodes
{
    public class NodeListItem : AdfChildNode
    {
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

        public NodeListItem(List<AdfTopNode> content) : base(NodeTypeChild.listItem)
        {
            Content = content;
        }

        private void ValidateContent(List<AdfTopNode> content)
        {
            bool flag = content.All(x => x.Type == NodeTypeTop.paragraph
                || x.Type == NodeTypeTop.bulletList
                || x.Type == NodeTypeTop.orderedList
                || x.Type == NodeTypeTop.codeBlock
                || x.Type == NodeTypeTop.mediaSingle);

            if (!flag)
            {
                throw new ArgumentException("NodeListItem: Content can only child the following nodes: NodeParagraph, NodeBulletList, NodeOrderedList, NodeCodeBlock, or NodeMediaSingle");
            }
        }
    }
}
