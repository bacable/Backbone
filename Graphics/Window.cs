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
        private Movable3D BackPanel { get; set; }
        private TextGroup Header { get; set; }
        #endregion

        public Window(WindowSettings<T> settings)
        {
            Size = settings.Size;

            if(BackgroundModel == null)
            {
                throw new NullReferenceException("Windows Background Model not set");
            }

            BackPanel = new Movable3D(BackgroundModel, settings.Position, settings.BackPanelScale.X);

            settings.Header.Parent = BackPanel;

            Header = new TextGroup(settings.Header);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            BackPanel.Draw(view, projection);
            Header.Draw(view, projection);
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
            BackPanel.Update(gameTime);
            Header.Update(gameTime);
        }
    }
}
