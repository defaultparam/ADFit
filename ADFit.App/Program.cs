using ADFit.Converters;
using ADFit.Models.Adf;
using ADFit.Models.Adf.Nodes;
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
    <p>World
    <p>New world</p>
    </p>
    <h1 style='color: green; background-color: black'>This is <b><i>italics and </i>bold</b> heading</h1>
    <span>Some text <span id="" ch"">asddf </span></span>
    <hr />
    <strong style='color: green; background-color: black'>Hello <i>World</i></strong><a href='https://google.com/'>
      Google</a>
    <p style='color: green; background-color: black'>This is <u
        style='color: blue; background-color: pink'>underlined</u> paragraph</p>
    <h2>This is <i>italic</i>heading</h2> World
    <ul>
      <li style='color: green; background-color: black'>Hello <i>World</i></li>
      <li><strong style='color: green; background-color: black'>Created by paramjot</strong></li>
      <li>Hello
        <ol>
          <li>Meow OL</li>
        </ol>
      </li>
    </ul>
  </div>
  <table>
    <tr>
      <th style='color: green; background-color: black'>Firstname</th>
      <th>Lastname</th>
      <th>Age</th>
    </tr>
    <tr>
      <td style='color: green; background-color: black'>Paramjot</td>
      <td>Singh</td>
      <td>23</td>
    </tr>
    <tr>
      <td>
        <ul>
          <li>Hello<i>World</i></li>
          <li><strong style='color: green; background-color: black'>Created by paramjot</strong></li>
          <li>Hello
            <ol>
              <li>Meow OL</li>
            </ol>
          </li>
        </ul>
      </td>
      <td>Singh</td>
      <td>21</td>
    </tr>
  </table>
  Other world
  <pre>
    function addNums(a, b) {
    return a + b
    }
  </pre>
  <blockquote>I am a mess even after so many hours of coding
    <ul>
      <li>MeowMeowPlease</li>
    </ul>
  </blockquote>

<details>
    <summary>Details Expand from HTML</summary>
  <h1>Expand</h1>
  <p>Expand content</p>
</details>
</body>
</html>";

        var tempHtml = @"<body><p style='color: hsl(200,12,43)'>This is <u style='color: blue; background-color: pink'>underlined</u> paragraph</p></body>";
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
}