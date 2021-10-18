using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Graphics
{
    public class Window<T> : IGUI3D
    {
        #region Static Globals
        public static Model Background { get; set; }
        #endregion

        #region Instance Properties
        private Rectangle Bounds { get; private set; }
        #endregion

        public Window(WindowSettings<T> windowSettings)
        {
            Bounds = windowSettings.Bounds;

        }

        public void Draw(Matrix view, Matrix projection)
        {
        }

        public void HandleMouse(Vector2 mousePosition, Matrix view, Matrix projection, Viewport viewport)
        {
        }

        public void TransitionIn()
        {
        }

        public void TransitionOut()
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
