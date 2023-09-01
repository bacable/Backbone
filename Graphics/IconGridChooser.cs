using Backbone.Actions;
using Backbone.Graphics;
using Backbone.Input;
using Backbone.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximityND.Backbone.Graphics
{
    public class IconGridChooser : IGUI3D
    {
        private Dictionary<string, IInteractive> valueToIcons = new Dictionary<string, IInteractive>();

        public float? OverrideCollisionRadius = null;

        private Dictionary<InputAction, IInteractive> inputToIcons = new Dictionary<InputAction, IInteractive>();

        private IInteractive selectedIcon;
        public IInteractive SelectedIcon
        {
            get
            {
                return selectedIcon;
            }
        }

        public Func<IAction3D> ClickAnimation { get; set; } = null;

        private string id;
        private float gapSize = 0.0f;
        private float iconSize = 0.0f;
        private int iconsPerRow = 1;
        private Vector3 basePosition = Vector3.Zero;

        public IconGridChooser(MenuOptionChooser chooser, IconGridChooserSettings settings)
        {
            id = settings.Id;
            iconsPerRow = settings.IconsPerRow;
            gapSize = settings.GapSize;
            iconSize = settings.IconSize;
            basePosition = settings.Position;
        }

        public void AddIcons(List<(string value, IInteractive icon, InputAction action)> icons)
        {
            foreach (var icon in icons)
            {
                int id = valueToIcons.Count;

                valueToIcons[icon.value] = icon.icon;
                inputToIcons[icon.action] = icon.icon;

                int column = id % iconsPerRow;
                int row = (int)Math.Floor((double)(id / iconsPerRow));

                icon.icon.UpdatePosition(new Vector3(
                    basePosition.X + column * (iconSize + gapSize),
                    basePosition.Y - row * (iconSize + gapSize),
                    0f));
            }
        }

        public void SetValue(IInteractive newValue)
        {
            selectedIcon = newValue;
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            if (command.State == MouseEvent.Release)
            {
                foreach(var icon in valueToIcons)
                {
                    if(icon.Value.IsInteractive && icon.Value.Intersects(command.Viewport, command.WorldPosition, Vector2.Zero, command.Ratio, OverrideCollisionRadius))
                    {
                        Debug.WriteLine($"Update icon choice: {icon.Key}");
                        SetValue(icon.Value);
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
            foreach (var icon in valueToIcons)
            {
                icon.Value.Draw(view, projection);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(var input in inputToIcons)
            {
                if(InputHelper.IsKeyUp(input.Key))
                {
                    var newSelected = valueToIcons.FirstOrDefault(x => x.Value == input.Value).Value;
                    if(newSelected != null)
                    {
                        Debug.WriteLine($"Update icon choice: {input.Key}");
                        SetValue(newSelected);
                    }
                    break;
                }
            }

            foreach( var icon in valueToIcons)
            {
                icon.Value.Update(gameTime);
            }
        }
    }
}
