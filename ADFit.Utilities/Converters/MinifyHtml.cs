
using System.Text.RegularExpressions;

namespace ADFit.Utilities.Converters;
public static class SanitizeHtml
{
    public static string MinifyHtml(this string html)
    {
        // Define a regex pattern to match <pre> tags and capture their inner content
        var preTagPattern = @"(<pre.*?>)(.*?)(</pre>)";

        // Use Regex.Matches to find <pre> tags and their inner content
        var preTags = Regex.Matches(html, preTagPattern, RegexOptions.Singleline);

        // Replace <pre> tags with placeholders to preserve their content during minification
        var placeholders = new List<string>();
        foreach (Match match in preTags)
        {
            var preTagStart = match.Groups[1].Value; // <pre> tag start
            var preTagEnd = match.Groups[3].Value;   // </pre> tag end
            var preContent = match.Groups[2].Value; // Inner content of <pre>

            // Store only the pre content as placeholder, excluding <pre> tags
            placeholders.Add(preContent);
            html = html.Replace(match.Value, $"{preTagStart}##PRE_PLACEHOLDER_{placeholders.Count - 1}##{preTagEnd}");
        }

        // Minify the rest of the HTML
        var minifiedHtml = Regex.Replace(html, @"\s+", " "); // Replace multiple spaces with a single space
        minifiedHtml = Regex.Replace(minifiedHtml, @">\s+<", "><"); // Remove whitespace between tags

        // Replace placeholders back with their original <pre> content
        for (int i = 0; i < placeholders.Count; i++)
        {
            // Restore pre content inside <pre> tags
            minifiedHtml = minifiedHtml.Replace($"##PRE_PLACEHOLDER_{i}##", placeholders[i]);
        }

        return minifiedHtml.Trim(); // Trim any extra spaces at the start or end
    }

}
