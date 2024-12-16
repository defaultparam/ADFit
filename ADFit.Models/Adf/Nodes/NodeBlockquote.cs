namespace ADFit.Models.Adf.Nodes
{
    public class NodeBlockquote : AdfTopNode
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

        public NodeBlockquote(List<AdfTopNode> content) : base(NodeTypeTop.blockquote)
        {
            Content = content;
        }

        private void ValidateContent(List<AdfTopNode> content)
        {
            if (content == null || content.Count == 0)
            {
                throw new ArgumentException("NodeBlockquote: Content must contain 1 or more elements");
            }

            bool isValid = content.All(node =>
                node.Type == NodeTypeTop.paragraph ||
                node.Type == NodeTypeTop.bulletList ||
                node.Type == NodeTypeTop.orderedList ||
                node.Type == NodeTypeTop.codeBlock ||
                node.Type == NodeTypeTop.mediaGroup ||
                node.Type == NodeTypeTop.mediaSingle);

            if (!isValid)
            {
                throw new ArgumentException("NodeBlockquote: Content can only child the following nodes: NodeParagraph, NodeBulletList, NodeOrderedList, NodeCodeBlock, NodeMediaGroup, and NodeMediaSingle");
            }
        }
    }
}
