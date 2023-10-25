using Backbone.Actions;
using Backbone.Events;
using Backbone.Input;
using Backbone.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProximityND.Backbone.Graphics;
using ProximityND.GUI3D.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Vector3 InactivePosition { get; set; }
        public Vector3 ActivePosition { get; set; }
        public float TransitionDuraton { get; set; }

        public bool IsAnimating
        {
            get
            {
                return BackPanel.IsAnimating;
            }
        }

        // movables that can be added to the window and interacted with
        private Dictionary<string, Movable3D> decorations = new Dictionary<string, Movable3D>();
        public Movable3D GetDecoration(string id)
        {
            return decorations[id] ?? null;
        }

        public Action OnClick;
        private float onClickDiameter;

        // menu stuff
        private OptionGroup optionGroup;

        private WindowSettings<T> settings;

        private MenuButton closeButton;

        #endregion

        public Window(WindowSettings<T> settings)
        {
            this.settings = settings;

            InactivePosition = settings.InactivePosition;
            ActivePosition = settings.ActivePosition;
            TransitionDuraton = settings.TransitionDuration;

            Size = settings.Size;

            if(BackgroundModel == null)
            {
                throw new NullReferenceException("Windows Background Model not set");
            }
            BackPanel = new Movable3D(BackgroundModel, settings.InactivePosition, settings.BackPanelScale.X);

            if(settings.Header != null)
            {
                settings.Header.Parent = BackPanel;

                Header = new TextGroup(settings.Header);
            }

            onClickDiameter = settings.OnClickDiameter;

            if(settings.Menu != null)
            {
                Menu = settings.Menu;
                var oGroupSettings = new OptionGroupSettings()
                {
                    Menu = settings.Menu,
                    ParentMovable = BackPanel,
                    Position = settings.MenuPosition,
                    SelectedColor = settings.SelectedColor,
                    UnselectedColor = settings.UnselectedColor,
                };
                optionGroup = new OptionGroup(oGroupSettings);
            }

            if(settings.ShowCornerCloseButton)
            {
                if(settings.CloseButtonAction != null)
                {
                    throw new Exception("Close button action cannot be null if enabled");
                }

                closeButton = new MenuButton("close", settings.CloseButtonAction);
            }
        }

        public void Run(IAction3D action, bool replaceExisting = false)
        {
            BackPanel.Run(action, replaceExisting);
        }

        /// <summary>
        /// Set as parent to given movable so its position will match the window's position
        /// </summary>
        /// <param name="movable">Movable to attach.</param>
        public void Attach(Movable3D movable)
        {
            movable.Parent = BackPanel;
        }

        public void AddDecoration(string id, Movable3D movable)
        {
            movable.Parent = this.BackPanel;
            decorations.Add(id, movable);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            if(settings.VisibleWhileInactive || IsActive || IsAnimating)
            {
                BackPanel.Draw(view, projection);

                if (Header != null)
                {
                    Header.Draw(view, projection);
                }

                optionGroup.Draw(view, projection);

                foreach(var key in decorations.Keys)
                {
                    decorations[key].Draw(view, projection);
                }
            }
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            if(!settings.VisibleWhileInactive || command.State == MouseEvent.Release)
            {
                // Handle click of backpanel, if set up to do something
                // TODO: need to make this a 2D rectangular collision instead of ray to sphere collision. Okay-ish for now,
                // but not good enough for release
                if (Collision3D.Intersects(command.MousePosition, BackPanel.Model, BackPanel.World, command.View, command.Projection, command.Viewport, onClickDiameter))
                {
                    if(IsActive)
                    {
                        optionGroup.HandleMouse(command);
                    }
                    else
                    {
                        OnClick?.Invoke();
                    }
                } else
                {
                    if(IsActive)
                    {
                        OnClick?.Invoke();
                    }
                }
            }
        }

        internal void ClickMenu()
        {
            if(IsActive && !IsAnimating)
            {
                Menu.Click();
            }
        }

        /// <summary>
        /// Update header text and possibly color (if passed in)
        /// </summary>
        /// <param name="newHeaderText"></param>
        /// <param name="color"></param>
        public void UpdateHeader(string newHeaderText, string color = "")
        {
            if(color != string.Empty)
            {
                Header.SetColor(color);
            }

            Header.SetText(newHeaderText);
        }

        /// <summary>
        /// If no animation provided, use the standard move in from offscreen and set as active
        /// </summary>
        /// <param name="animation">Animation to perform on the backpanel</param>
        public void Activate(IAction3D animation = null)
        {
            IsActive = true;
            if(animation == null)
            {
                animation = MoveAnim(settings.InactivePosition, settings.ActivePosition);
            }
            BackPanel.Run(animation);
        }

        internal void Deactivate(IAction3D animation = null)
        {
            IsActive = false;
            if (animation == null)
            {
                animation = MoveAnim(settings.ActivePosition, settings.InactivePosition);
            }
            BackPanel.Run(animation, true);
        }

        private IAction3D MoveAnim(Vector3 from, Vector3 to)
        {
            var moveTo = ActionBuilder.MoveTo(to, settings.TransitionDuration);
            return moveTo;
        }

        public void TransitionIn()
        {

        }

        public void TransitionOut()
        {

        }

        public void Reposition(Vector3 newPosition)
        {
            BackPanel.Position = newPosition;
        }

        public void Update(GameTime gameTime)
        {
            if(settings.VisibleWhileInactive || IsActive || IsAnimating)
            {
                if (!IsAnimating && IsActive)
                {
                    UpdateMenuInput();
                }

                BackPanel.Update(gameTime);

                if (Header != null)
                {
                    Header.Update(gameTime);
                }

                optionGroup.Update(gameTime);

                foreach(var key in decorations.Keys)
                {
                    var decoration = decorations[key];
                    decoration.Update(gameTime);
                }
            }
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
