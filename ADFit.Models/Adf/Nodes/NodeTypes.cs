namespace ADFit.Models.Adf.Nodes
{
    public enum NodeTypes
    {
        NodeTypeTop,
        NodeTypeChild,
        NodeTypeInline
    }
    public enum NodeTypeTop
    {
        blockquote,
        bulletList,
        codeBlock,
        expand,
        heading,
        mediaGroup,
        mediaSingle,
        orderedList,
        panel,
        paragraph,
        rule,
        table
    }

    public enum NodeTypeChild
    {
        listItem,
        media,
        nestedExpand,
        tableCell,
        tableHeader,
        tableRow
    }

    public enum NodeTypeInline
    {
        date,
        emoji,
        hardBreak,
        inlineCard,
        mention,
        status,
        text
    }
}
