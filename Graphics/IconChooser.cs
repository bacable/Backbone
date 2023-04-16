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

        public Action<string> OnChange
        {
            get
            {
                return chooser.OnChange;
            }
            set
            {
                chooser.OnChange = value;
            }
        }

        private string _id;

        public IInteractive Icon { get
            {
                return valueToIcons[chooser.SelectedOption.Value] ?? null;
            }
        }

        public string Value
        {
            get { return chooser.SelectedOption.Value; }
        }

        public IconChooser(MenuOptionChooser chooser, string id)
        {
            this.chooser = chooser;
            _id = id;
        }

        public void AddIcons(List<(string value, IInteractive icon)> icons)
        {
            foreach (var icon in icons)
            {
                chooser.Add(new MenuOption(icon.value, icon.value));
                valueToIcons[icon.value] = icon.icon;
            }
        }

        public void SetValue(string newValue)
        {
            this.chooser.SetValue(newValue);
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            if(command.State == MouseEvent.Release && Icon != null && Icon.IsInteractive)
            {
                if (Icon.Intersects(command.Viewport, command.WorldPosition, Vector2.Zero, command.Ratio, OverrideCollisionRadius))
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
