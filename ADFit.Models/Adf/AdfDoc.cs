using ADFit.Models.Adf.Nodes;

namespace ADFit.Models.Adf
{
    public class AdfDoc
    {
        public int Version = 1;

        public string Type = "doc";
        
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
