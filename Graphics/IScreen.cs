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
        void Draw(Matrix view, Matrix projection);
        void DrawText(SpriteBatch spriteBatch);
        void HandleMouse(HandleMouseCommand command);

        void Resize(int newWidth, int newHeight);
        void Cleanup();
    }
}
