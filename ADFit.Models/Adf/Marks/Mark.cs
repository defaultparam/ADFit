namespace ADFit.Models.Adf.Marks
{
    public abstract class Mark
    {
        public MarkTypes Type { get; set; }
        protected Mark(MarkTypes type)
        {
            Type = type;
        }
    }

    public enum MarkTypes
    {
        backgroundColor,
        code,
        em,
        link,
        strike,
        strong,
        subsup,
        textColor,
        underline
    }
}
