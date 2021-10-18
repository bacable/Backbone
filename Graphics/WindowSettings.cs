using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Graphics
{
    public class WindowSettings<T>
    {
        public T Type { get; set; }
        public Rectangle Bounds { get; set; }
    }
}
