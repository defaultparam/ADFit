using System.Drawing;

namespace ADFit.Utilities.Converters
{
    public class ColorConverter
    {
        // Convert RGB to HEX
        public static string FromRgbToHex(int r, int g, int b)
        {
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        // Convert RGBA to HEX
        public static string FromRgbaToHex(int r, int g, int b, float a)
        {
            int alpha = (int)(a * 255);
            return $"#{r:X2}{g:X2}{b:X2}{alpha:X2}";
        }

        // Convert HEXA to HEX
        public static string FromHexaToHex(string hexa)
        {
            if (hexa.Length == 9) // #RRGGBBAA
            {
                return hexa.Substring(0, 7); // Strip alpha channel
            }
            throw new ArgumentException("Invalid hexa format");
        }

        // Convert HSL to HEX
        public static string FromHslToHex(double h, double s, double l)
        {
            var (r, g, b) = HslToRgb(h, s, l);
            return FromRgbToHex(r, g, b);
        }

        // Convert HSLA to HEX
        public static string FromHslaToHex(double h, double s, double l, float a)
        {
            var (r, g, b) = HslToRgb(h, s, l);
            return FromRgbaToHex(r, g, b, a);
        }

        // Convert Named Color to HEX
        public static string FromNamedColorToHex(string colorName)
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

        // Helper method to convert HSL to RGB
        private static (int r, int g, int b) HslToRgb(double h, double s, double l)
        {
            double c = (1 - Math.Abs(2 * l - 1)) * s;
            double x = c * (1 - Math.Abs(h / 60 % 2 - 1));
            double m = l - c / 2;

            double r, g, b;
            if (h >= 0 && h < 60) { r = c; g = x; b = 0; }
            else if (h >= 60 && h < 120) { r = x; g = c; b = 0; }
            else if (h >= 120 && h < 180) { r = 0; g = c; b = x; }
            else if (h >= 180 && h < 240) { r = 0; g = x; b = c; }
            else if (h >= 240 && h < 300) { r = x; g = 0; b = c; }
            else { r = c; g = 0; b = x; }

            int R = (int)((r + m) * 255);
            int G = (int)((g + m) * 255);
            int B = (int)((b + m) * 255);

            return (R, G, B);
        }
    }
}