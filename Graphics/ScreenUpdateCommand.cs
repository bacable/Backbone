using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Backbone.Graphics
{
    public class ScreenUpdateCommand
    {
        public GameTime GameTime { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Viewport Viewport { get; set; }
    }
}
