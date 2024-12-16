using System.ComponentModel.DataAnnotations;

namespace ADFit.Models.Adf.Marks
{
    public class MarkBackgroundColor : Mark
    {
        public Attributes Attrs { get; set; }
        public MarkBackgroundColor(string color) : base(MarkTypes.backgroundColor)
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
