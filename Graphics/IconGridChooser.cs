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

        private Movable3D cursor = null;

        private IInteractive selectedIcon;
        public IInteractive SelectedIcon
        {
            get
            {
                return selectedIcon;
            }
        }

        public Func<IAction3D> ClickAnimation { get; set; } = null;

        public Action<string> OnChange { get; set; } = null;

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
            cursor = new Movable3D(settings.CursorModel, new Vector3(600f, 0f, 25f), settings.CursorScale);
            cursor.AddMeshProperties(settings.CursorProperties);
            OverrideCollisionRadius = settings.OverrideCollisionRadius;
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

            if(icons.Count > 0)
            {
                cursor.UpdatePosition(icons[0].icon.GetPosition());
            }
        }

        public void SetValue(IInteractive newValue)
        {
            if(newValue != selectedIcon)
            {
                selectedIcon = newValue;
                cursor.UpdatePosition(selectedIcon.GetPosition());
                OnChange.Invoke(newValue.Name);
            }
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            if (command.State == MouseEvent.Release)
            {
                foreach(var icon in valueToIcons)
                {
                    if(icon.Value.IsInteractive && icon.Value.Intersects(command.Viewport, command.WorldPosition, Vector2.Zero, command.Ratio, OverrideCollisionRadius))
                    {
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

            cursor.Draw(view, projection);
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
