
namespace ADFit.Models.Adf.Nodes
{
    public class NodeMediaGroup : AdfTopNode
    {
        private List<NodeMedia> _content = [];
        public List<NodeMedia> Content
        {
            get => _content;
            set
            {
                ValidateContent(value);
                _content = value;
            }
        }

        public NodeMediaGroup(List<NodeMedia> content) : base(NodeTypeTop.mediaGroup)
        {
            Content = content;
        }

        private void ValidateContent(List<NodeMedia> content)
        {
            if (content.Count == 0)
            {
                throw new ArgumentException("Content must contain 1 or more elements");
            }
        }
    }
}
