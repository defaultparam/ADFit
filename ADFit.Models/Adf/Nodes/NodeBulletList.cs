

namespace ADFit.Models.Adf.Nodes
{
    public class NodeBulletList : AdfTopNode
    {
        private List<NodeListItem> _content = [];
        public List<NodeListItem> Content
        {
            get => _content;
            set
            {
                ValidateContent(value);
                _content = value;
            }
        }

        public NodeBulletList(List<NodeListItem> content) : base(NodeTypeTop.bulletList)
        {
            Content = content;
        }

        private void ValidateContent(List<NodeListItem> content)
        {
            if (content is null || content.Count == 0)
            {
                throw new ArgumentException("NodeBulletList: Content must contain 1 or more elements");
            }
        }
    }
}
