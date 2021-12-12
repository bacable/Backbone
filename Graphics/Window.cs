using Backbone.Actions;
using Backbone.Input;
using Backbone.Menus;
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
        private MenuContainer Menu { get; set; }

        public bool IsActive { get; set; } = false;

        public bool IsAnimating
        {
            get
            {
                return BackPanel.IsAnimating;
            }
        }

        public Action OnClick;
        private float onClickDiameter;

        // menu stuff
        private OptionGroup optionGroup;

        #endregion

        public Window(WindowSettings<T> settings)
        {
            Size = settings.Size;

            if(BackgroundModel == null)
            {
                throw new NullReferenceException("Windows Background Model not set");
            }
            BackPanel = new Movable3D(BackgroundModel, settings.Position, settings.BackPanelScale.X);

            if(settings.Header != null)
            {
                settings.Header.Parent = BackPanel;

                Header = new TextGroup(settings.Header);
            }

            onClickDiameter = settings.OnClickDiameter;

            if(settings.Menu != null)
            {
                Menu = settings.Menu;
                optionGroup = new OptionGroup(settings.MenuPosition, settings.Menu, BackPanel);
            }
        }

        public void Run(IAction3D action, bool replaceExisting = false)
        {
            BackPanel.Run(action, replaceExisting);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            BackPanel.Draw(view, projection);

            if(Header != null)
            {
                Header.Draw(view, projection);
            }

            optionGroup.Draw(view, projection);
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            // Handle click of backpanel, if set up to do something
            // TODO: need to make this a 2D rectangular collision instead of ray to sphere collision. Okay-ish for now,
            // but not good enough for release
            if(Collision3D.Intersects(command.MousePosition, BackPanel.Model, BackPanel.World, command.View, command.Projection, command.Viewport, onClickDiameter))
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
            if(!IsAnimating && IsActive)
            {
                UpdateMenuInput();
            }

            BackPanel.Update(gameTime);


            if(Header != null)
            {
                Header.Update(gameTime);
            }

            optionGroup.Update(gameTime);
        }

        private void UpdateMenuInput()
        {
            if (InputHelper.IsKeyUp(InputAction.Down))
            {
                Menu.Next();
            }
            else if (InputHelper.IsKeyUp(InputAction.Up))
            {
                Menu.Prev();
            }

            if (InputHelper.IsKeyUp(InputAction.Left))
            {
                if (Menu.CanPrev)
                {
                    Menu.PrevOption();
                }
            }
            else if (InputHelper.IsKeyUp(InputAction.Right))
            {
                if (Menu.CanNext)
                {
                    Menu.NextOption();
                }
            }
        }
    }
}
