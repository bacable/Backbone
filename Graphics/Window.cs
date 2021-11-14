using Backbone.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProximityND.Backbone.Graphics;
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

        public bool IsAnimating
        {
            get
            {
                return BackPanel.IsAnimating;
            }
        }

        public Action OnClick;
        private float onClickDiameter;

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

            onClickDiameter = settings.OnClickDiameter;
        }

        public void Run(IAction3D action, bool replaceExisting = false)
        {
            BackPanel.Run(action, replaceExisting);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            BackPanel.Draw(view, projection);
            Header.Draw(view, projection);
        }

        public void HandleMouse(Vector2 mousePosition, Matrix view, Matrix projection, Viewport viewport)
        {
            // Handle click of backpanel, if set up to do something
            // TODO: need to make this a 2D rectangular collision instead of ray to sphere collision. Okay-ish for now,
            // but not good enough for release
            if(Collision3D.Intersects(mousePosition, BackPanel.Model, BackPanel.World, view, projection, viewport, onClickDiameter))
            {
                OnClick?.Invoke();
            }
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
