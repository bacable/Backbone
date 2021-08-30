using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Backbone.Graphics
{
    public interface IGUI3D
    {
        void Update(GameTime gameTime);
        void Draw(Matrix view, Matrix projection);
        void HandleMouse(Vector2 mousePosition, Matrix view, Matrix projection, Viewport viewport);
        void TransitionIn();
        void TransitionOut();
    }
}
