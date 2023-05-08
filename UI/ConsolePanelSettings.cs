using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Backbone.UI
{
    public class ConsolePanelSettings
    {
        public Rectangle Bounds { get; set; }
        public int MaxLines { get; set; }
        public float SecondsBeforeLineExpiration { get; set; }
        public Vector2 StartPosition { get; set; }
        public SpriteFont Font { get; set; }
        public Color TextColor { get; set; }
        public float TextScale { get; set; }
        public float LineHeight { get; set; }
        public float FadeSpeed { get; set; } = 1f;
        public float TransitionDistance { get; set; } = 30f;
    }
}
