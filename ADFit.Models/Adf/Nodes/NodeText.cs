using ADFit.Models.Adf.Marks;

namespace ADFit.Models.Adf.Nodes
{
    public class NodeText : AdfInlineNode
    {
        public string Text { get; set; }

        private List<Mark>? _marks;
        public List<Mark>? Marks
        {
            get => _marks;
            set
            {
                ValidateMarks(value);
                _marks = value;
            }
        }
        public NodeText(string text) : base(NodeTypeInline.text)
        {
            Text = text;
        }

        private void ValidateMarks(List<Mark>? marks)
        {
            if (marks == null)
            {
                return;
            }
            // Implement check for no bg color mark with code mark.
            foreach (var mark in marks)
            {
                if (mark.Type == MarkTypes.code)
                {
                    throw new ArgumentException("code mark is not allowed in text node");
                }
            }
        }
    }
}
