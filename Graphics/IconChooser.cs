using Backbone.Input;
using Backbone.Menus;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Backbone.Graphics
{
    public class IconChooser : IGUI3D
    {
        private Dictionary<string, ICollidableDrawable> valueToIcons = new Dictionary<string, ICollidableDrawable>();

        private MenuOptionChooser chooser;

        public ICollidableDrawable Icon { get
            {
                return valueToIcons[chooser.SelectedOption.Value] ?? null;
            }
        }

        public IconChooser(MenuOptionChooser chooser)
        {
            this.chooser = chooser;
        }

        public void AddIcons(List<(string value, ICollidableDrawable icon)> icons)
        {
            foreach (var icon in icons)
            {
                valueToIcons[icon.value] = icon.icon;
            }
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            Debug.WriteLine("Handle Mouse Called");
            if(Icon != null)
            {
                Debug.WriteLine("Icon not null");
                if (Icon.Intersects(command.MousePosition, command.View, command.Projection, command.Viewport))
                {
                    Debug.WriteLine("It intersects!");
                    this.chooser.Next();
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
