using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Backbone.Graphics
{


    public interface IGUI3D
    {
        void Update(GameTime gameTime);
        void Draw(Matrix view, Matrix projection);
        public void DrawUnderText(Matrix view, Matrix projection)
        {
            //Empty default implementation
        }

        void HandleMouse(HandleMouseCommand command);
        void TransitionIn();
        void TransitionOut();
        public void Dispose()
        {
            //Empty default implementation
        }
    }
}

