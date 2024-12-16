using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ADFit.Models.Adf.Nodes
{
    public class NodeDate : AdfInlineNode
    {
        public Attributes Attrs { get; set; }

        public NodeDate(DateTime date) : base(NodeTypeInline.date)
        {
            Attrs = new Attributes(date);
        }

        public NodeDate(long timestamp) : base(NodeTypeInline.date)
        {
            Attrs = new Attributes(timestamp);
        }

        public class Attributes
        {
            public string Timestamp { get; }

            public Attributes(DateTime date)
            {
                var _date = ((DateTimeOffset)date).ToUnixTimeSeconds();
                Timestamp = _date.ToString();
            }

            public Attributes(long timestamp)
            {
                Timestamp = timestamp.ToString();
            }
        }
    }
}
