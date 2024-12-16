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
        doc.LoadHtml(html.MinifyHtml());

        var bodyNode = doc.DocumentNode.SelectSingleNode("//body");
        if (bodyNode == null)
        {
            throw new ArgumentException("Invalid HTML: No <body> tag found");
        }
        return new AdfDoc(ConvertTopNode(bodyNode));
    }

    private static NodeTypes? GetNodeType(HtmlNode htmlNode)
    {
        switch (htmlNode.Name)
        {
            case "p":
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
                return NodeTypes.NodeTypeTop;

            case "#text":
            case "span":
            case "br":
            case "strong":
            case "em":
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

    private static AdfChildNode GetChildNode(HtmlNode htmlNode, dynamic content)
    {
        return htmlNode.Name switch
        {
            "li" => new NodeListItem(content),
            "tr" => new NodeTableRow(content),
            "th" => new NodeTableHeader(content),
            "td" => new NodeTableCell(content),
            //"img" => NodeTypeChild.media,
            _ => throw new InvalidOperationException("Unsupported child node type")
        };
    }

    private static object GetInlineNode(HtmlNode htmlNode)
    {
        List<AdfInlineNode> nodes = new List<AdfInlineNode>();
        return htmlNode.Name switch
        {
            "#text" => new NodeText(htmlNode.InnerText),
            "span" => new NodeText(htmlNode.InnerText),
            "br" => new NodeHardBreak(),
            "strong" => new MarkStrong(),
            "em" => new MarkEm(),
            "code" => new MarkCode(),
            "a" => new MarkLink(htmlNode.Attributes.AttributesWithName("href").FirstOrDefault()?.Value ?? "#"),
            "s" => new MarkStrike(), // strike
            "del" => new MarkStrike(), // strike
            "u" => new MarkUnderline(),
            "sup" => new MarkSubsup(MarkSubsup.SubsupType.Sup),
            "sub" => new MarkSubsup(MarkSubsup.SubsupType.Sub),
            _ => throw new InvalidOperationException("Unsupported inline node type")
        };
    }

    private static List<AdfTopNode> ConvertTopNode(HtmlNode node, List<AdfTopNode>? parentTopNodes = null)
    {
        List<AdfTopNode> content = new();
        var nodesList = node.ChildNodes;

        // TECHNICALLY... THIS IS NOT CONSIDERED VALID SYNTAX IN HTML, BUT HEY! PEOPLE ARE DUMB :upsidedown:
        List<HtmlNode> rootLevelInlines = [];

        foreach (var childNode in nodesList)
        {
            if (childNode.Name == "div")
            {
                content.AddRange(ConvertTopNode(childNode, parentTopNodes));
                break;
            }
            //if (inlineChildNode.== "#text")
            //{
            //    break;
            //}
            NodeTypes? nodeType = GetNodeType(childNode);
            if (nodeType is NodeTypes.NodeTypeTop)
            {
                if (childNode.ChildNodes.Count == 0 || GetNodeType(childNode.FirstChild) == NodeTypes.NodeTypeInline)
                {
                    // Don't traverse
                    List<AdfInlineNode> childNodes = ConvertInlineNodes(childNode);
                    AdfTopNode topNode = GetTopNode(childNode, childNodes);
                    content.Add(topNode);
                }
                else
                {
                    // Traverse
                    content.AddRange(ConvertTopNode(childNode, parentTopNodes));
                }
            }
            // Exception: If the first element inside to `body` is a text
            else if (nodeType is NodeTypes.NodeTypeInline)
            {
                rootLevelInlines.Add(childNode);
                if (childNode.NextSibling != null && GetNodeType(childNode.NextSibling) is NodeTypes.NodeTypeInline)
                {
                    continue;
                }
                else
                {
                    HtmlNode paragraphNode = HtmlNode.CreateNode("<p></p>");
                    rootLevelInlines.ForEach(x => paragraphNode.AppendChild(x));
                    List<AdfInlineNode> childNodes = ConvertInlineNodes(paragraphNode);
                    NodeParagraph topNode = new NodeParagraph(childNodes);
                    content.Add(topNode);

                    rootLevelInlines.Clear();
                }
            }
            else
            {
                Console.WriteLine($"{node.Name} is not supported for ADF conversion");
            }
            //else if (nodeType is NodeTypes.NodeTypeChild)
            //{
            //    NodeTypeChild result = GetChildNode(node);
            //    if (node.ChildNodes.Count > 0)
            //    {
            //        ConvertChildren(node);
            //    }
            //    doc.Content.Add(ConvertNode(node));
            //}
        }

        return content;
    }

    private static List<AdfInlineNode> ConvertInlineNodes(HtmlNode parentElementNode, List<Mark>? parentMarks = null)
    {
        parentMarks ??= new List<Mark>(); // Initialize if null
        var inlineNodes = new List<AdfInlineNode>();

        foreach (var inlineChildNode in parentElementNode.ChildNodes)
        {
            if (inlineChildNode.NodeType == HtmlNodeType.Text)
            {
                inlineNodes.Add(new NodeText(inlineChildNode.InnerText) { Marks = new List<Mark>(parentMarks) });
            }
            else
            {
                inlineNodes.AddRange(AddMarks(inlineChildNode, parentMarks));
            }
        }
        return inlineNodes;
    }

    private static List<AdfInlineNode> AddMarks(HtmlNode node, List<Mark> parentMarks)
    {
        var updatedMarks = new List<Mark>(parentMarks); // Clone the parent's marks

        if (node.Name == "sub")
        {
            updatedMarks.Add(new MarkSubsup(MarkSubsup.SubsupType.Sub));
        }
        else if (node.Name == "sup")
        {
            updatedMarks.Add(new MarkSubsup(MarkSubsup.SubsupType.Sup));
        }
        else if (node.Name == "strong" || node.Name == "b")
        {
            updatedMarks.Add(new MarkStrong());
        }
        else if (node.Name == "em" || node.Name == "i")
        {
            updatedMarks.Add(new MarkEm());
        }
        else if (node.Name == "code")
        {
            updatedMarks.Add(new MarkCode());
        }
        else if (node.Name == "a")
        {
            var href = node.Attributes["href"]?.Value ?? "#";
            updatedMarks.Add(new MarkLink(href));
        }
        else if (node.Name == "s" || node.Name == "del")
        {
            updatedMarks.Add(new MarkStrike());
        }
        else if (node.Name == "u")
        {
            updatedMarks.Add(new MarkUnderline());
        }

        // Process child nodes recursively, passing the updated marks
        return ConvertInlineNodes(node, updatedMarks);
    }

    //public static AdfTopNode? ReturnTopNode(HtmlNode htmlNode)
    //{
    //    switch (htmlNode.Name)
    //    {
    //        case "p":
    //            return new NodeParagraph(ConvertInlineNodes(htmlNode));
    //        case "h1":
    //            return new NodeHeading(1, ConvertInlineNodes(htmlNode));
    //        case "h2":
    //            return new NodeHeading(2, ConvertInlineNodes(htmlNode));
    //        case "h3":
    //            return new NodeHeading(3, ConvertInlineNodes(htmlNode));
    //        case "h4":
    //            return new NodeHeading(4, ConvertInlineNodes(htmlNode));
    //        case "h5":
    //            return new NodeHeading(5, ConvertInlineNodes(htmlNode));
    //        case "h6":
    //            return new NodeHeading(6, ConvertInlineNodes(htmlNode));
    //        //case "ul":
    //        //    return new NodeBulletList(ConvertChildNode(htmlNode));
    //        //case "ol":
    //        //    return new NodeOrderedList(ConvertChildNode(htmlNode));
    //        //case "code":
    //        //    return new NodeCodeBlock(htmlNode.InnerText);
    //        case "hr":
    //            return new NodeRule();
    //        //case "table":
    //        //return new NodeTable(ConvertChildNode(htmlNode));
    //        case "blockquote":
    //            return new NodeBlockquote(ConvertChildNode(htmlNode));
    //        case "details":
    //            return new NodeExpand(ConvertChildNode(htmlNode));
    //        //case "img":
    //        //    return new NodeMediaSingle(htmlNode.Attributes["src"].Value);
    //        default:
    //            return null;
    //    }
    //}
}
