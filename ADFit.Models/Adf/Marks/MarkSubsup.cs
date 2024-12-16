namespace ADFit.Models.Adf.Marks
{
    public class MarkSubsup : Mark
    {
        public Attributes Attrs { get; set; }
        public MarkSubsup(SubsupType type) : base(MarkTypes.subsup)
        {
            Attrs = new Attributes(type);
        }

        public class Attributes
        {
            public SubsupType Type { get; set; }

            public Attributes(SubsupType type)
            {
                Type = type;
            }
        }

        public enum SubsupType
        {
            Sub,
            Sup
        }
    }
}
