namespace ADFit.Models.Adf.Nodes
{
    public class NodeMediaSingle : AdfTopNode
    {
        public Attributes Attrs { get; set; }
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

        private void ValidateContent(List<NodeMedia> content)
        {
            if (content.Count != 1)
            {
                throw new ArgumentException("Content must contain a single elemnent");
            }

            foreach (var media in content)
            {
                if (media.Attrs?.Width == null || media.Attrs?.Height == null)
                {
                    throw new ArgumentException("NodeMediaSingle: For NodeMediaSingle, width and height attributes must be provided within NodeMedia or the media isn't displayed.");
                }
            }
        }

        public NodeMediaSingle(List<NodeMedia> content, MediaLayout layout) : base(NodeTypeTop.mediaSingle)
        {
            Content = content;
            Attrs = new Attributes(layout);
        }

        public NodeMediaSingle(List<NodeMedia> content, MediaLayout layout, float? width, WidthType widthType = WidthType.percentage) : base(NodeTypeTop.mediaSingle)
        {
            Content = content;
            Attrs = new Attributes(layout, width, widthType);
        }

        public class Attributes
        {
            public MediaLayout Layout { get; }
            public float? Width { get; }
            public WidthType? WidthType { get; }

            public Attributes(MediaLayout layout)
            {
                Layout = layout;
            }

            public Attributes(MediaLayout layout, float? width, WidthType widthType = NodeMediaSingle.WidthType.percentage) : this(layout)
            {
                Width = width;
                WidthType = widthType;
            }
        }

        public enum MediaLayout
        {
            wrap_left,
            center,
            wrap_right,
            wide,
            full_width,
            align_start,
            align_end
        }
        public enum WidthType
        {
            pixel,
            percentage
        }
    }
}
