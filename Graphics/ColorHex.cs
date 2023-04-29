using Microsoft.Xna.Framework;
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

    }
}
