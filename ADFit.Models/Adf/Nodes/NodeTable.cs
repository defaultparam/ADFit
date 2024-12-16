namespace ADFit.Models.Adf.Nodes
{
    public class NodeTable : AdfTopNode
    {
        public Attributes? Attrs { get; set; } = null;
        public List<NodeTableRow> Content { get; set; }

        public NodeTable(List<NodeTableRow> content) : base(NodeTypeTop.table)
        {
            // one or more should be present
            Content = content;
        }
        public NodeTable(List<NodeTableRow> content, bool? isNullColumnEnabled = null, int? width = null, Layout? layout = null, DisplayMode? displayMode = null) : this(content)
        {
            Attrs = new Attributes(isNullColumnEnabled, width, layout, displayMode);
        }


        public class Attributes
        {
            public bool? IsNumberColumnEnabled { get; }
            public int? Width { get; }
            public Layout? Layout { get; }
            public DisplayMode? DisplayMode { get; }

            public Attributes(bool? isNullColumnEnabled = null, int? width = null, Layout? layout = null, DisplayMode? displayMode = null)
            {
                IsNumberColumnEnabled = isNullColumnEnabled;
                Width = width;
                Layout = layout;
                DisplayMode = displayMode;
            }
        }

        public enum Layout
        {
            center,
            align_start
        }

        public enum DisplayMode
        {
            DEFAULT,
            FIXED
        }
    }
}
