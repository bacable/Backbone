using Backbone.Actions;
using Backbone.Input;
using Backbone.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Backbone.Graphics
{
    public class IconChooser : IGUI3D
    {
        private Dictionary<string, IInteractive> valueToIcons = new Dictionary<string, IInteractive>();

        private MenuOptionChooser chooser;

        public float? OverrideCollisionRadius = null;

        public Func<IAction3D> ClickAnimation { get; set; } = null;

        public IInteractive Icon { get
            {
                return valueToIcons[chooser.SelectedOption.Value] ?? null;
            }
        }

        public IconChooser(MenuOptionChooser chooser)
        {
            this.chooser = chooser;
        }

        public void AddIcons(List<(string value, IInteractive icon)> icons)
        {
            foreach (var icon in icons)
            {
                valueToIcons[icon.value] = icon.icon;
            }
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            if(command.State == MouseEvent.Release && Icon != null && Icon.IsInteractive)
            {
                if (Icon.Intersects(command.MousePosition, command.View, command.Projection, command.Viewport, OverrideCollisionRadius))
                {
                    this.chooser.Next();
                    if(ClickAnimation != null)
                    {
                        Icon.Run(ClickAnimation());
                    }
                }
            }
        }

        public void TransitionIn()
        {
        }

        public void TransitionOut()
        {
        }

        public void Draw(Matrix view, Matrix projection)
        {
            if(Icon != null)
            {
                Icon.Draw(view, projection);
            }
        }

        public void Update(GameTime gameTime)
        {
            if(Icon != null)
            {
                Icon.Update(gameTime);
            }
        }
    }
}
