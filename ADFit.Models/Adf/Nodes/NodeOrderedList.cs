namespace ADFit.Models.Adf.Nodes
{
    public class NodeOrderedList : AdfTopNode
    {
        public List<NodeListItem> Content { get; set; }
        public NodeOrderedList(List<NodeListItem> content) : base(NodeTypeTop.orderedList)
        {
            if (content.Count == 0)
            {
                throw new ArgumentException("Content must contain 1 or more elements");
            }
            Content = content;
        }
    }
}
