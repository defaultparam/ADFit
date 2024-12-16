using System.ComponentModel.DataAnnotations;

namespace ADFit.Models.Adf.Marks
{
    public class MarkTextColor : Mark
    {
        public Attributes Attrs { get; set; }
        public MarkTextColor(string color) : base(MarkTypes.textColor)
        {
            Attrs = new Attributes(color);
        }

        public class Attributes
        {
            [RegularExpression("^#[0-9a-fA-F]{6}$")]
            public string Color { get; set; }

            public Attributes(string color)
            {
                Color = color;
            }
        }
    }
}
