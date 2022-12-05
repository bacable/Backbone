using Microsoft.Xna.Framework;

namespace Backbone.Graphics
{
    public interface IDrawable
    {
        void Draw(Matrix view, Matrix projection);
        void Update(GameTime gameTime);
    }
}
