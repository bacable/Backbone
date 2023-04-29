using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public class ColorType3D
    {
        private static Dictionary<ColorType, Vector3> Colors = new Dictionary<ColorType, Vector3>()
        {
            { ColorType.Red, new Vector3(0.7391f,0.1159f, 0.1449f) },
            { ColorType.Blue, new Vector3(0.32f, 0.32f, 0.66f) },
            { ColorType.Orange, new Vector3(0.6741f, 0.4542f, 0.1718f) },
            { ColorType.Purple, new Vector3(0.4662f, 0.2972f, 0.5366f) },
            { ColorType.Yellow, new Vector3(0.7f, 0.7f, 0f) },
            { ColorType.Green, new Vector3(0.2510f, 0.7007f, 0.3483f) },
            { ColorType.Gray, new Vector3(0.4333f, 0.4333f, 0.4333f) },
            { ColorType.LightGray, new Vector3(0.7f, 0.7f, 0.7f) },
            { ColorType.DefaultText, new Vector3(0.75f, 0.75f, 0.75f) },
            { ColorType.White, new Vector3(0.9f, 0.9f, 0.9f) },
            { ColorType.LightOrange, new Vector3(0.882f, 0.694f, 0.06274f) },
            { ColorType.Gold, new Vector3(0.85f, 0.8f, 0.06274f) },
            { ColorType.Black, new Vector3(0.1f, 0.1f, 0.1f) },
            { ColorType.Pink, new Vector3(0.7686f, 0.2471f, 0.7529f) },
            { ColorType.Cyan, new Vector3(0.0235f, 0.7686f, 0.8275f) }

        };

        public static Dictionary<ColorType, string> ColorHexValues = new Dictionary<ColorType, string>()
        {
            { ColorType.Red, "#D65539" },
            { ColorType.Blue, "#5252a8" },
            { ColorType.Orange, "#ac752c" },
            { ColorType.Purple, "#774c89" },
            { ColorType.Yellow,  "#b3b300"},
            { ColorType.Green, "#40b359" },
            { ColorType.Gray, "#6e6e6e" },
            { ColorType.LightGray, "#b3b3b3" },
            { ColorType.DefaultText, "#bfbfbf" },
            { ColorType.White, "#e6e6e6" },
            { ColorType.LightOrange, "#d1b110" },
            { ColorType.Gold, "#D9CC10" },
            { ColorType.Black, "#1A1A1A" },
            { ColorType.Pink, "#C43FC0" },
            { ColorType.Cyan, "#06C4D3" }
        };

        public static Vector3 Get(ColorType color)
        {
            return Colors[color];
        }
    }
}
