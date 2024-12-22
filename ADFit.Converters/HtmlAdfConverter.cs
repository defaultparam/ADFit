using ADFit.Models.Adf;
using ADFit.Models.Adf.Marks;
using ADFit.Models.Adf.Nodes;
using ADFit.Utilities.Converters;
using HtmlAgilityPack;

namespace ADFit.Converters;

public class HtmlAdfConverter
{
    public static AdfDoc ConvertHtmlToAdf(string html)
    {
        var doc = new HtmlDocument();
        //Console.WriteLine($"HTML document:\n\t {html}");
        doc.LoadHtml(html.MinifyHtml());
        var bodyNode = doc.DocumentNode.SelectSingleNode("//body");
        if (bodyNode == null)
        {
            throw new ArgumentException("Invalid HTML: No <body> tag found");
        }
        return new AdfDoc(ConvertTopNode(bodyNode));
    }

    private static List<AdfTopNode> ConvertTopNode(HtmlNode node)
    {
        List<AdfTopNode> content = new();
        List<Mark> parentMarks = [];
        if (node.Attributes.Contains("style"))
        {
            parentMarks = ExtractColorStyles(node);
        }

        var nodesList = node.ChildNodes;
        List<HtmlNode> rootLevelInlines = [];

        foreach (var childNode in nodesList)
        {
            List<Mark> childMarks = new(parentMarks);
            // Checks whether node has any color styles applied to it.
            if (childNode.Attributes.Contains("style"))
            {
                childMarks.AddRange(ExtractColorStyles(childNode));
            }

            if (childNode.Name == "img")
            {
                // Media types required them to be uploaded first and then reference their collection and id here...
                continue;
            }
            else
            {
                NodeTypes? nodeType = GetNodeType(childNode);

                if (nodeType is NodeTypes.NodeTypeTop)
                {
                    if (childNode.Name == "div")
                    {
                        content.AddRange(ConvertTopNode(childNode));
                        continue;
                    }
                    else if (childNode.Name == "blockquote")
                    {
                        content.Add(HandleBlockquotes(childNode));
                        continue;
                    }
                    else if (childNode.Name == "details" || childNode.Name == "summary")
                    {
                        if (childNode.Name == "summary")
                        {
                            continue;
                        }
                        content.Add(HandleExpands(childNode));
                        continue;
                    }
                    else if (childNode.Name == "pre")
                    {
                        content.Add(HandleCodeblocks(childNode));
                    }
                    else if (childNode.Name == "ul" || childNode.Name == "ol")
                    {
                        content.Add(HandleLists(childNode));
                        continue;
                    }
                    else if (childNode.Name == "table")
                    {
                        NodeTable? table = HandleTables(childNode);
                        if (table is null) continue;

                        content.Add(table);
                        continue;
                    }
                    else if (childNode.ChildNodes.Count == 0 || GetNodeType(childNode.FirstChild) == NodeTypes.NodeTypeInline)
                    {
                        List<AdfInlineNode> childNodes = ConvertInlineNodes(childNode, childMarks);
                        AdfTopNode topNode = GetTopNode(childNode, childNodes);
                        content.Add(topNode);
                    }
                    else
                    {
                        content.AddRange(ConvertTopNode(childNode));
                    }
                }
                // If an inline element is direct child to `body`
                else if (nodeType is NodeTypes.NodeTypeInline)
                {
                    // Grouping all the inline elements in a series into a paragraph and then constructing ADF object.
                    rootLevelInlines.Add(childNode);
                    if (childNode.NextSibling != null && GetNodeType(childNode.NextSibling) is NodeTypes.NodeTypeInline)
                    {
                        // Will keep grouping the inline elements in the series in `rootLevelInlines` list.
                        continue;
                    }
                    else
                    {
                        // Putting collected inline nodes in a paragraph and then converting to ADF nodes.
                        HtmlNode paragraphNode = HtmlNode.CreateNode("<p></p>");
                        rootLevelInlines.ForEach(x => paragraphNode.AppendChild(x));

                        List<AdfInlineNode> childNodes = ConvertInlineNodes(paragraphNode, childMarks);
                        NodeParagraph topNode = new NodeParagraph(childNodes);
                        content.Add(topNode);

                        // Clearing the rootLevelInline list for next series of inline elements.
                        rootLevelInlines.Clear();
                    }
                }
                else
                {
                    Console.WriteLine($"{node.Name} is not supported for ADF conversion");
                }
            }
        }

        return content;
    }

    private static NodeTypes? GetNodeType(HtmlNode htmlNode)
    {
        switch (htmlNode.Name)
        {
            case "p":
            case "div":
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
            case "hr":
            case "ul":
            case "ol":
            case "blockquote":
            case "table":
            case "pre":
            case "details":
            case "summary":
                return NodeTypes.NodeTypeTop;

            case "#text":
            case "br":
            case "span":
            case "strong":
            case "b":
            case "em":
            case "i":
            case "code":
            case "a":
            case "s":
            case "del":
            case "u":
            case "sup":
            case "sub":
                return NodeTypes.NodeTypeInline;

            default:
                return null;
        }
    }

    private static AdfTopNode GetTopNode(HtmlNode htmlNode, List<AdfInlineNode> content)
    {
        return htmlNode.Name switch
        {
            "p" => new NodeParagraph(content),
            "div" => new NodeParagraph(content),
            "h1" => new NodeHeading(1, content),
            "h2" => new NodeHeading(2, content),
            "h3" => new NodeHeading(3, content),
            "h4" => new NodeHeading(4, content),
            "h5" => new NodeHeading(5, content),
            "h6" => new NodeHeading(6, content),
            "hr" => new NodeRule(),
            //"pre" => new NodeCodeBlock(content),
            //"span" => new NodeParagraph(content),
            //"ul" => new NodeBulletList(content),
            //"ol" => new NodeOrderedList(content),
            //"table" => new NodeTable(content),
            //"blockquote" => new NodeBlockquote(content),
            //"details" => new NodeExpand(content),

            //"code" => NodeTypeTop.codeBlock, // IT IS INLINE
            //"img" => NodeTypeTop.mediaSingle, // IT IS SUPPOSED TO BE A CHILD?
            _ => throw new InvalidOperationException("Unsupported top node type")
        };
    }

    private static NodeExpand HandleExpands(HtmlNode parentNode)
    {
        if (!parentNode.HasChildNodes)
        {
            return new NodeExpand([]);
        }
        if (parentNode.ChildNodes.All(cn => GetNodeType(cn) == NodeTypes.NodeTypeTop || cn.Name == "summary"))
        {
            string title = parentNode.SelectSingleNode("summary")?.InnerText ?? "Expand";
            List<AdfTopNode> content = ConvertTopNode(parentNode);
            return new NodeExpand(title, content);
        }
        else
        {
            throw new Exception("NodeExpand: Invalid HTML. Must contain atleast 1 or more supported top level nodes.");
        }

    }

    private static NodeBlockquote HandleBlockquotes(HtmlNode parentNode)
    {
        if (!parentNode.HasChildNodes)
        {
            return new NodeBlockquote([]);
        }
        if (parentNode.ChildNodes.All(cn =>
            cn.Name == "#text" ||
            cn.Name == "p" ||
            cn.Name == "pre" ||
            cn.Name == "ol" ||
            cn.Name == "ul" ||
            cn.Name == "img"
        ))
        {
            List<AdfTopNode> content = new();
            foreach (var childNode in parentNode.ChildNodes)
            {
                if (childNode.Name == "#text")
                {
                    content.Add(new NodeParagraph([new NodeText(childNode.InnerText)]));
                }
                if (childNode.Name == "p" || childNode.Name == "pre")
                {
                    content.AddRange(ConvertTopNode(childNode));
                }
                if (childNode.Name == "ul" || childNode.Name == "ol")
                {
                    content.Add(HandleLists(childNode));
                }
                if (childNode.Name == "img")
                {
                    Console.WriteLine("Support for image tags requires reference id of uploaded attachment to JIRA account. Skipping...");
                    continue;
                }
            }
            return new NodeBlockquote(content); ;
        }
        else
        {
            throw new Exception("NodeBlockquotes: Invalid HTML. Only <p>, <ol>/<ul>, <pre>, <img> items are allowed as children.");
        }

    }

    private static NodeCodeBlock HandleCodeblocks(HtmlNode parentNode)
    {
        if (!parentNode.HasChildNodes)
        {
            return new NodeCodeBlock([]);
        }
        if (parentNode.ChildNodes.All(cn => cn.Name == "#text"))
        {
            var childNodes = parentNode.FirstChild.InnerText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            List<NodeText> content = new();
            foreach (var childNode in childNodes)
            {
                var lineText = new NodeText(childNode + "\r\n");
                content.Add(lineText);
            }
            return new NodeCodeBlock(content);
        }
        else
        {
            throw new Exception("NodeCodeblock: Invalid HTML. Only pre-formatted items are allowed as children.");
        }

    }

    private static AdfTopNode HandleLists(HtmlNode parentNode)
    {
        if (!parentNode.HasChildNodes)
        {
            return parentNode.Name == "ul" ? new NodeBulletList([]) : new NodeOrderedList([]);
        }
        if (parentNode.ChildNodes.All(cn => cn.Name == "li"))
        {
            List<NodeListItem> listItems = new();
            foreach (var childNode in parentNode.ChildNodes)
            {
                var content = HandleListItems(childNode);
                NodeListItem listItem = new NodeListItem(content);
                listItems.Add(listItem);
            }
            return parentNode.Name == "ul" ? new NodeBulletList(listItems) : new NodeOrderedList(listItems);
        }
        else
        {
            throw new Exception("NodeBulletList || NodeOrderedList: Invalid HTML. Only <li> items are allowed as children.");
        }
    }

    private static List<AdfTopNode> HandleListItems(HtmlNode liNode)
    {
        if (liNode.ChildNodes.Count == 0)
        {
            return [new NodeParagraph([])];
        }

        var liInlines = new List<HtmlNode>();
        var listItemContent = new List<AdfTopNode>();

        foreach (var childNode in liNode.ChildNodes)
        {
            if (GetNodeType(childNode) == NodeTypes.NodeTypeInline)
            {
                liInlines.Add(childNode);
                if (childNode.NextSibling != null && GetNodeType(childNode.NextSibling) is NodeTypes.NodeTypeInline)
                {
                    // Will keep grouping the inline elements in the series in `liInlines` list.
                    continue;
                }
                else
                {
                    // Putting collected inline nodes in a paragraph and then converting to ADF nodes.
                    HtmlNode paragraphNode = HtmlNode.CreateNode("<p></p>");
                    liInlines.ForEach(x => paragraphNode.AppendChild(x));
                    List<AdfInlineNode> childNodes = ConvertInlineNodes(paragraphNode);
                    NodeParagraph topNode = new NodeParagraph(childNodes);

                    // Clearing the rootLevelInline list for next series of inline elements.
                    liInlines.Clear();

                    listItemContent.Add(topNode);
                }

            }
            if (childNode.Name == "ul" || childNode.Name == "ol")
            {
                listItemContent.Add(HandleLists(childNode));
            }
            else if (childNode.Name == "p" || childNode.Name == "div")
            {
                // THROW EXCEPTIONS... UR NOT SUPPOSED TO HAVE <p> OR <div> INSIDE LI's!!
                listItemContent.AddRange(ConvertTopNode(childNode));
            }
            else if (childNode.Name == "table")
            {

            }
            else if (childNode.Name == "img")
            {
                //listItemContent.Add(new NodeMediaSingle(childNode.Attributes["src"].Value));
            }

        }
        return listItemContent;
    }

    private static NodeTable? HandleTables(HtmlNode parentNode)
    {
        if (!parentNode.HasChildNodes)
        {
            return null;
        }
        if (parentNode.ChildNodes.All(cn => cn.Name == "tr"))
        {
            List<NodeTableRow> tableRows = new();
            foreach (var childNode in parentNode.ChildNodes)
            {
                var content = HandleTableRows(childNode);

                tableRows.Add(content);
            }
            return new NodeTable(tableRows);
        }
        else
        {
            throw new Exception("NodeTable: Invalid HTML. Only <tr> items are allowed as children.");
        }
    }

    private static NodeTableRow HandleTableRows(HtmlNode tableRow)
    {
        if (!tableRow.HasChildNodes)
        {
            throw new Exception("NodeTableRow: Table row must contain 1 or more NodeTableHeader | NodeTableCell");
        }
        if (tableRow.ChildNodes.All(cn => cn.Name == "th" || cn.Name == "td"))
        {
            List<AdfChildNode> tableCells = new();
            foreach (var childNode in tableRow.ChildNodes)
            {
                var content = HandleTableHeaderOrCells(childNode);
                tableCells.Add(content);
            }
            return new NodeTableRow(tableCells);
        }
        else
        {
            throw new Exception("NodeTableRow: Invalid HTML. Only <th> or <td> items are allowed as children.");
        }
    }

    private static AdfChildNode HandleTableHeaderOrCells(HtmlNode tableHeaderOrCell)
    {
        if (!tableHeaderOrCell.HasChildNodes)
        {
            throw new Exception("Table cell must contain 1 or more top level elements");
        }
        List<AdfTopNode> content = ConvertTopNode(tableHeaderOrCell);

        return tableHeaderOrCell.Name == "th" ? new NodeTableHeader(content) : new NodeTableCell(content);
    }

    private static List<AdfInlineNode> ConvertInlineNodes(HtmlNode parentNode, List<Mark>? parentMarks = null)
    {
        parentMarks ??= new List<Mark>();
        var inlineNodes = new List<AdfInlineNode>();
        // 1. Will iterate over all the child nodes of a parent element node.
        foreach (var inlineNode in parentNode.ChildNodes)
        {
            if (inlineNode.NodeType == HtmlNodeType.Text)
            {
                // 5. Once got to the text node, it applies all the parent marks collected so far in the nested manner.
                parentMarks.AddRange(ExtractColorStyles(inlineNode));
                inlineNodes.Add(new NodeText(inlineNode.InnerText) { Marks = new List<Mark>(parentMarks) });
                // 6. Then returns the list of inline nodes back to it's parent's inlineNodes list.
            }
            else if (inlineNode.Name == "br")
            {
                inlineNodes.Add(new NodeHardBreak());
            }
            else
            {
                // 2. Will be called if the child node is an inline element and not a text node.
                inlineNodes.AddRange(AddMarks(inlineNode, parentMarks));
                // 7. when return adds the range of collected inline nodes in the same hierarchy (as siblings) in the parent's inlineNodes list.
            }
        }

        // Returns only the list, in case there are nested tags like bold and italics.
        return inlineNodes;
    }

    private static List<AdfInlineNode> AddMarks(HtmlNode inlineNode, List<Mark> parentMarks)
    {
        // 3. Checks and creates a list of marks applied to the text at current level
        var updatedMarks = new List<Mark>(parentMarks);
        if (inlineNode.Attributes.Contains("style"))
        {
            updatedMarks.AddRange(ExtractColorStyles(inlineNode));
        }


        switch (inlineNode.Name)
        {
            case "sub":
                updatedMarks.Add(new MarkSubsup(MarkSubsup.SubsupType.Sub));
                break;
            case "sup":
                updatedMarks.Add(new MarkSubsup(MarkSubsup.SubsupType.Sup));
                break;
            case "strong":
            case "b":
                updatedMarks.Add(new MarkStrong());
                break;
            case "em":
            case "i":
                updatedMarks.Add(new MarkEm());
                break;
            case "code":
                updatedMarks.Add(new MarkCode());
                break;
            case "a":
                var href = inlineNode.Attributes["href"]?.Value ?? "#";
                updatedMarks.Add(new MarkLink(href));
                break;
            case "s":
            case "del":
                updatedMarks.Add(new MarkStrike());
                break;
            case "u":
                updatedMarks.Add(new MarkUnderline());
                break;
        }

        // 4. Process child nodes recursively, passing the updated marks
        return ConvertInlineNodes(inlineNode, updatedMarks);
    }

    private static List<Mark> ExtractColorStyles(HtmlNode node)
    {
        List<Mark> colorMarks = [];
        var style = node.GetAttributeValue("style", "");
        var styles = ParseStyleAttribute(style);

        if (styles.TryGetValue("color", out string? textColor))
        {
            colorMarks.Add(new MarkTextColor(textColor));
        }
        if (styles.TryGetValue("background-color", out string? backgroundColor))
        {
            colorMarks.Add(new MarkBackgroundColor(backgroundColor));
        }

        return colorMarks;
    }

    private static Dictionary<string, string> ParseStyleAttribute(string style)
    {
        var styles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var stylePairs = style.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var pair in stylePairs)
        {
            var keyValue = pair.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim();
                string value = keyValue[1].Trim().ConvertColorToHex();
                styles[key] = value;
            }
        }

        return styles;
    }
}