namespace ADFit.Models.Adf.Nodes
{
    public class NodeMention : AdfInlineNode
    {
        public Attributes Attrs { get; set; }
        public NodeMention(string id) : base(NodeTypeInline.mention)
        {
            Attrs = new Attributes(id);
        }
        public NodeMention(string accountId, string? text, UserType? userType, AccessLevel? accessLevel) : base(NodeTypeInline.mention)
        {
            Attrs = new Attributes(accountId, text, userType, accessLevel);
        }

        public class Attributes
        {
            public string Id { get; }
            public string? Text { get; }
            public UserType? UserType { get; }
            public AccessLevel? AccessLevel { get; }

            public Attributes(string id)
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException("NodeMention: AccountId cannot be null or empty");
                }
                Id = id;
            }

            public Attributes(string id, string? text, UserType? userType, AccessLevel? accessLevel) : this(id)
            {
                Text = text;
                UserType = userType;
                AccessLevel = accessLevel;
            }
        }

        public enum UserType
        {
            DEFAULT,
            SPECIAL,
            APP
        }
        public enum AccessLevel
        {
            NONE,
            SITE,
            APPLICATION,
            CONTAINER
        }
    }
}
