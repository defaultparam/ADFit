
namespace ADFit.Models.Adf.Nodes
{
    public class NodeTableRow : AdfChildNode
    {
        private List<AdfChildNode> _content = [];
        public List<AdfChildNode> Content
        {
            get => _content;
            set
            {
                ValidateContent(value);
                _content = value;
            }
        }

        public NodeTableRow(List<AdfChildNode> content) : base(NodeTypeChild.tableRow)
        {
            Content = content;
        }

        private void ValidateContent(List<AdfChildNode> content)
        {
            if (content is null || content.Count == 0)
            {
                throw new ArgumentException("NodeTableRow: Content must contain 1 or more elements");
            }
            bool flag = content.All(content => content.Type == NodeTypeChild.tableCell || content.Type == NodeTypeChild.tableHeader);
            if (!flag)
            {
                throw new ArgumentException("NodeTableRow: Content can only child the following nodes: NodeTableHeader and NodeTableCell");
            }
        }
    }
}
