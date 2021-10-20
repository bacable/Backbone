using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Graphics
{
    public class WindowSettings<T>
    {
        public string HeaderText { get; set; }
        public T Type { get; set; }
        public Rectangle Size { get; set; }
        public Vector3 Position { get; set; }
    }
}
