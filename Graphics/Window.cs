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
        public static Model BackgroundModel { get; set; }
        #endregion

        #region Instance Properties
        private Rectangle Size { get; set; }
        private Movable3D Background { get; set; }
        #endregion

        public Window(WindowSettings<T> windowSettings)
        {
            Size = windowSettings.Size;

            if(BackgroundModel == null)
            {
                throw new NullReferenceException("Windows Background Model not set");
            }

            Background = new Movable3D(BackgroundModel, windowSettings.Position, 10.0f);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Background.Draw(view, projection);
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
            Background.Update(gameTime);
        }
    }
}
