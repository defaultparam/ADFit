using ADFit.Models.Adf.Marks;

namespace ADFit.Models.Adf.Nodes
{
    public class NodeMedia : AdfChildNode
    {
        public Attributes Attrs { get; set; }
        private List<Mark>? _marks = null;
        public List<Mark>? Marks
        {
            get => _marks;
            set
            {
                ValidateMarks(value);
                _marks = value;
            }
        }

        public NodeMedia(string id, MediaType type, string collection, List<Mark>? marks = null) : base(NodeTypeChild.media)
        {
            Attrs = new Attributes(id, type, collection);
            Marks = marks;
        }

        public NodeMedia(string id, MediaType type, string collection, int? width = null, int? height = null, string? occurrenceKey = null, List<Mark>? marks = null) : base(NodeTypeChild.media)
        {
            Attrs = new Attributes(id, type, collection, width, height, occurrenceKey);
            Marks = marks;
        }

        public class Attributes
        {
            public string Id { get; }
            public MediaType Type { get; }
            public string Collection { get; }
            public int? Width { get; }
            public int? Height { get; }
            public string? OccurrenceKey { get; }

            public Attributes(string id, MediaType type, string collection)
            {
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(collection))
                {
                    throw new ArgumentException("NodeMedia: Id and collection cannot be null or empty");
                }
                Id = id;
                Type = type;
                Collection = collection;
            }

            public Attributes(string id, MediaType type, string collection, int? width, int? height, string? occurrenceKey) : this(id, type, collection)
            {
                Width = width;
                Height = height;
                OccurrenceKey = occurrenceKey;
            }
        }

        private void ValidateMarks(List<Mark>? marks)
        {
            if (marks != null)
            {
                bool flag = marks.All(x => x.Type == MarkTypes.link);
                if (!flag)
                {
                    throw new ArgumentException("NodeMedia: Invalid mark type in the array");
                }
            }
        }

        public enum MediaType
        {
            file,
            link
        }
    }
}
