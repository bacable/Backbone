using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProximityND.Backbone.Graphics
{
    public class VariableMonitorSettings
    {
        public Color Color { get; set; } = Color.White;
        public SpriteFont Font { get; set; }
        public float Height { get; set; }
        public float Scale { get; set; } = 1f;
        public Vector2 Position { get; set; } = Vector2.Zero;
    }
}
