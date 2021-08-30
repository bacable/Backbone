using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Graphics
{
    public static class GraphicStore
    {
        public static Dictionary<string, Effect> Effects { get; set; } = new Dictionary<string, Effect>();
    }
}
