namespace ADFit.Models.Adf.Nodes
{
    public abstract class Node<T>
    {
        public T Type { get; set; }
        protected Node(T type)
        {
            Type = type;
        }
    }

    public abstract class AdfTopNode(NodeTypeTop type) : Node<NodeTypeTop>(type);
    public abstract class AdfChildNode(NodeTypeChild type) : Node<NodeTypeChild>(type);
    public abstract class AdfInlineNode(NodeTypeInline type) : Node<NodeTypeInline>(type);

}
