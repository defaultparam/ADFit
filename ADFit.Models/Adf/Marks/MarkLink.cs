namespace ADFit.Models.Adf.Marks
{
    public class MarkLink : Mark
    {
        public Attributes Attrs { get; set; }
        public MarkLink(string href) : base(MarkTypes.link)
        {
            Attrs = new Attributes(href);
        }

        public MarkLink(string href, string title) : base(MarkTypes.link)
        {
            Attrs = new Attributes(href, title);
        }

        public class Attributes
        {
            //[Required]
            public string Href { get; set; }
            public string? Title { get; set; } // When mouse hover, it will show the title of the Uri
            public string? Collection { get; set; }
            public string? Id { get; set; }
            public string? OccurrenceKey { get; set; }

            public Attributes(string href)
            {
                Href = href;
            }

            public Attributes(string href, string title)
            {
                Href = href;
                Title = title;
            }
        }
    }
}
