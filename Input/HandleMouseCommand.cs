using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Backbone.Input
{
    public class HandleMouseCommand
    {
        public Vector2 MousePosition { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Viewport Viewport { get; set; }
        public MouseEvent State { get; set; }
    }
}
