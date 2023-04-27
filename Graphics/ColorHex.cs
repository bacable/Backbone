using Backbone.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximityND.Backbone.Graphics
{
    public class ColorHex
    {
        private static Dictionary<string, Color> Colors = new Dictionary<string, Color>();

        private static Color Get(string hexCode)
        {
            if(Colors.ContainsKey(hexCode)) {
                return Colors[hexCode];
            } else
            {
                var color = System.Drawing.ColorTranslator.FromHtml(hexCode);
                Colors[hexCode] = color;
                return color;
            }
        }

    }
}
