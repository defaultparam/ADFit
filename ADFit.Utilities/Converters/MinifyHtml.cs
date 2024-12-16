namespace ADFit.Utilities.Converters;
public static class SanitizeHtml
{
    public static string MinifyHtml(this string html)
    {
        return html
            .Replace("\r", "") // Remove carriage returns
            .Replace("\n", "") // Remove newlines
            .Replace("\t", "") // Remove tabs
            .Trim(); // Trim leading and trailing whitespaces
    }

}
