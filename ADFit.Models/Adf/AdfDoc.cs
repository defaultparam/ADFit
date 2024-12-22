using ADFit.Models.Adf.Nodes;

namespace ADFit.Models.Adf
{
    public class AdfDoc
    {
        public const int version = 1;
        public const string type = "doc";

        public int Version { get; } = version;
        public string Type { get; } = type;
        public List<AdfTopNode> Content { get; set; } = [];
        
        public AdfDoc(List<AdfTopNode> content)
        {
            if (content != null)
            {
                Content = content;
            }
        }
    }
}
