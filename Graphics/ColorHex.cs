using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Drawing;

namespace Backbone.Graphics
{
    public class ColorHex
    {
        private static Dictionary<string, Vector3> Colors = new Dictionary<string, Vector3>();

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
