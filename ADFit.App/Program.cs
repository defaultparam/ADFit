using ADFit.Converters;
using ADFit.Models.Adf;
using ADFit.Utilities.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

class Program
{
    static void Main()
    {
        var html =
        @"<!DOCTYPE html>
<html>
<body>
Meow
<div>Hello
	        <h1>This is <b><i>italics and </i>bold</b> heading</h1>
        <span> Some text<span id=""ch"">asddf</span>
        </span><a href='https://google.com/'>Google</a>
        <p>This is <u>underlined</u> paragraph</p>
	        <h2>This is <i>italic</i> heading</h2> World
</div>
Other world
</body>
</html>";

        var tempHtml = @"<!DOCTYPE html>
<html>
<body>
<a href='https://google.com/'>Google</a>
</body>
</html>";
        AdfDoc adf = HtmlAdfConverter.ConvertHtmlToAdf(html);
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = {
                new StringEnumConverter(),
                new CamelCaseEnumConverter()
            },
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        string jsonString = JsonConvert.SerializeObject(adf, settings);
        Console.WriteLine(jsonString);
    }

    public static string MinifyHtml(string html)
    {
        return html
            .Replace("\r", "") // Remove carriage returns
            .Replace("\n", "") // Remove newlines
            .Replace("\t", "") // Remove tabs
            .Trim(); // Trim leading and trailing whitespaces
    }
}