using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Backbone.Graphics
{
    public class ColorHex
    {
        private static Dictionary<string, Vector3> Colors = new Dictionary<string, Vector3>();

        public static Dictionary<ColorType, string> DefaultColorHexCodes = new Dictionary<ColorType, string>()
        {
            { ColorType.Red, "#BC1E26" },
            { ColorType.Blue, "#5252A8" },
            { ColorType.Orange, "#AC752C" },
            { ColorType.Purple, "#774C89" },
            { ColorType.Yellow,  "#B3B300"},
            { ColorType.Green, "#40B359" },
            { ColorType.Gray, "#6E6E6E" },
            { ColorType.LightGray, "#B3B3B3" },
            { ColorType.DefaultText, "#BFBFBF" },
            { ColorType.White, "#E6E6E6" },
            { ColorType.LightOrange, "#D1B110" },
            { ColorType.Gold, "#D9CC10" },
            { ColorType.Black, "#1A1A1A" },
            { ColorType.Pink, "#C43FC0" },
            { ColorType.Cyan, "#06C4D3" }
        };

        public static Vector3 Get(string hexCode)
        {
            if(Colors.ContainsKey(hexCode)) {
                return Colors[hexCode];
            } else
            {
                var color = System.Drawing.ColorTranslator.FromHtml(hexCode);
                var colorVector = new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
                Colors[hexCode] = colorVector;
                return colorVector;
            }
        }

        public static string ConvertFromColor(Microsoft.Xna.Framework.Color color)
        {
            var systemDrawingColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            return System.Drawing.ColorTranslator.ToHtml(systemDrawingColor);
        }

        public static Microsoft.Xna.Framework.Color ConvertFromHex(string s, float alpha = 1.0f)
        {
            if (s.Length != 7 || !s.StartsWith("#"))
                return Microsoft.Xna.Framework.Color.Gray;

            int r = Convert.ToInt32(s.Substring(1, 2), 16);
            int g = Convert.ToInt32(s.Substring(3, 2), 16);
            int b = Convert.ToInt32(s.Substring(5, 2), 16);
            int a = Math.Clamp(Convert.ToInt32(alpha * 255), 0, 255);
            return new Microsoft.Xna.Framework.Color(r, g, b, a);
        }

        public static string DarkenColor(string color, float percentage)
        {
            return ConvertFromColor(Microsoft.Xna.Framework.Color.Lerp(ConvertFromHex(color), Microsoft.Xna.Framework.Color.Black, percentage));
        }

        public static Microsoft.Xna.Framework.Color DarkenColor(Microsoft.Xna.Framework.Color color, float percentage)
        {
            return Microsoft.Xna.Framework.Color.Lerp(color, Microsoft.Xna.Framework.Color.Black, percentage);
        }

        public static Microsoft.Xna.Framework.Color LightenColor(Microsoft.Xna.Framework.Color color, float percentage)
        {
            return Microsoft.Xna.Framework.Color.Lerp(color, Microsoft.Xna.Framework.Color.White, percentage);
        }

        public static string LightenColor(string color, float percentage)
        {
            return ConvertFromColor(Microsoft.Xna.Framework.Color.Lerp(ConvertFromHex(color), Microsoft.Xna.Framework.Color.White, percentage));
        }

        public static string AdjustSaturation(string originalColorHex, float saturationPercentage)
        {
            // Convert to Color
            var originalColor = ConvertFromHex(originalColorHex);

            // Convert the color to HSL
            float hue, saturation, lightness;
            RgbToHsl(originalColor, out hue, out saturation, out lightness);

            // Adjust the saturation
            saturation *= saturationPercentage;

            // Convert back to RGB
            var newColor = HslToRgb(hue, saturation, lightness, originalColor.A);

            return ConvertFromColor(newColor);
        }

        private static void RgbToHsl(Microsoft.Xna.Framework.Color color, out float hue, out float saturation, out float lightness)
        {
            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;

            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));

            lightness = (max + min) / 2f;

            if (max == min)
            {
                hue = 0f;
                saturation = 0f;
            }
            else
            {
                float delta = max - min;
                saturation = lightness > 0.5f ? delta / (2f - max - min) : delta / (max + min);

                if (max == r)
                {
                    hue = (g - b) / delta + (g < b ? 6f : 0f);
                }
                else if (max == g)
                {
                    hue = (b - r) / delta + 2f;
                }
                else
                {
                    hue = (r - g) / delta + 4f;
                }

                hue /= 6f;
            }
        }

        private static Microsoft.Xna.Framework.Color HslToRgb(float hue, float saturation, float lightness, byte alpha)
        {
            float r, g, b;

            if (saturation == 0f)
            {
                r = g = b = lightness; // achromatic
            }
            else
            {
                Func<float, float, float, float> hueToRgb = (p, q, t) =>
                {
                    if (t < 0f) t += 1f;
                    if (t > 1f) t -= 1f;
                    if (t < 1f / 6f) return p + (q - p) * 6f * t;
                    if (t < 1f / 3f) return q;
                    if (t < 2f / 3f) return p + (q - p) * (2f / 3f - t) * 6f;
                    return p;
                };

                float q = lightness < 0.5f ? lightness * (1f + saturation) : lightness + saturation - lightness * saturation;
                float p = 2f * lightness - q;

                r = hueToRgb(p, q, hue + 1f / 3f);
                g = hueToRgb(p, q, hue);
                b = hueToRgb(p, q, hue - 1f / 3f);
            }

            return new Microsoft.Xna.Framework.Color((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), alpha);
        }
    }
}
