using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public interface IScreen
    {
        List<IGUI3D> GuiElements { get; set; }
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(DrawLayerType layer, Matrix view, Matrix projection, SpriteBatch spriteBatch);
        void HandleMouse(HandleMouseCommand command);
        void Resize(int newWidth, int newHeight);
        void Cleanup();
    }
}
