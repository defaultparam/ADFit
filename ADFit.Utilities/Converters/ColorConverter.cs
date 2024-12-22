using System.Drawing;

namespace ADFit.Utilities.Converters
{
    public static class ColorConverter
    {
        public static string ConvertColorToHex(this string value)
        {
            if (value.StartsWith("#"))
            {
                if (value.Length == 7) // #RRGGBB
                {
                    return value;
                }
                else if (value.Length == 9) // #RRGGBBAA
                {
                    return FromHexaToHex(value);
                }
            }
            else if (value.StartsWith("rgb(") && value.EndsWith(")"))
            {
                var parts = value.Substring(4, value.Length - 5).Split(',');
                int r = int.Parse(parts[0].Trim());
                int g = int.Parse(parts[1].Trim());
                int b = int.Parse(parts[2].Trim());
                return FromRgbToHex(r, g, b);
            }
            else if (value.StartsWith("rgba(") && value.EndsWith(")"))
            {
                var parts = value.Substring(5, value.Length - 6).Split(',');
                int r = int.Parse(parts[0].Trim());
                int g = int.Parse(parts[1].Trim());
                int b = int.Parse(parts[2].Trim());
                float a = float.Parse(parts[3].Trim());
                return FromRgbaToHex(r, g, b, a);
            }
            else if (value.StartsWith("hsl(") && value.EndsWith(")"))
            {
                var parts = value.Substring(4, value.Length - 5).Split(',');
                double h = double.Parse(parts[0].Trim());
                double s = double.Parse(parts[1].Trim().TrimEnd('%')) / 100;
                double l = double.Parse(parts[2].Trim().TrimEnd('%')) / 100;
                (int r, int g, int b) = HslToRgb(h, s, l);

                // Convert RGB to Hex
                return $"#{r:X2}{g:X2}{b:X2}";
            }
            else if (value.StartsWith("hsla(") && value.EndsWith(")"))
            {
                var parts = value.Substring(5, value.Length - 6).Split(',');
                double h = double.Parse(parts[0].Trim());
                double s = double.Parse(parts[1].Trim().TrimEnd('%')) / 100;
                double l = double.Parse(parts[2].Trim().TrimEnd('%')) / 100;
                float a = float.Parse(parts[3].Trim());
                return FromHslaToHex(h, s, l, a);
            }
            else
            {
                return FromNamedColorToHex(value);
            }

            return "";
        }
        // Convert RGB to HEX
        public static string FromRgbToHex(int r, int g, int b)
        {
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        // Convert RGBA to HEX
        public static string FromRgbaToHex(int r, int g, int b, float a)
        {
            Console.WriteLine("WARNING: Converting RGBA to HEX... Will neglect alpha channels");
            int alpha = (int)(a * 255);
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        private static (int r, int g, int b) HslToRgb(double h, double s, double l)
        {
            double c = (1 - Math.Abs(2 * l - 1)) * s; // Chroma
            double x = c * (1 - Math.Abs((h / 60.0) % 2 - 1));
            double m = l - c / 2;
            Console.WriteLine("WARNING: Converting HSL / HSLA values to RGB / HEX... Color shades produced may not be accurate.");
            double rPrime = 0, gPrime = 0, bPrime = 0;

            if (h >= 0 && h < 60)
            {
                rPrime = c; gPrime = x; bPrime = 0;
            }
            else if (h >= 60 && h < 120)
            {
                rPrime = x; gPrime = c; bPrime = 0;
            }
            else if (h >= 120 && h < 180)
            {
                rPrime = 0; gPrime = c; bPrime = x;
            }
            else if (h >= 180 && h < 240)
            {
                rPrime = 0; gPrime = x; bPrime = c;
            }
            else if (h >= 240 && h < 300)
            {
                rPrime = x; gPrime = 0; bPrime = c;
            }
            else if (h >= 300 && h < 360)
            {
                rPrime = c; gPrime = 0; bPrime = x;
            }

            // Convert to 0-255 RGB range
            int r = (int)Math.Round((rPrime + m) * 255);
            int g = (int)Math.Round((gPrime + m) * 255);
            int b = (int)Math.Round((bPrime + m) * 255);

            return (r, g, b);
        }
        // Convert HSLA to HEX
        private static string FromHslaToHex(double h, double s, double l, float a)
        {
            Console.WriteLine("WARNING: Converting HSLA to HEX... Will neglect alpha channels and might produce a different color shade");
            var (r, g, b) = HslToRgb(h, s, l);
            return FromRgbaToHex(r, g, b, a);
        }

        // Convert Named Color to HEX
        private static string FromNamedColorToHex(string colorName)
        {
            try
            {
                Color color = Color.FromName(colorName);
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            catch
            {
                throw new ArgumentException("Invalid named color");
            }
        }

        // Convert HEXA to HEX
        private static string FromHexaToHex(string hexa)
        {
            Console.WriteLine("WARNING: Converting HEXA to HEX... Will neglect alpha channels");
            if (hexa.Length == 9) // #RRGGBBAA
            {
                return hexa.Substring(0, 7); // Strip alpha channel
            }
            throw new ArgumentException("Invalid hexa format");
        }
    }
}