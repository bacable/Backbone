using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Backbone.Actions
{
    internal class ChangeMovableSettingsAction : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        MovableSettings movableSettings;

        public ChangeMovableSettingsAction(MovableSettings movableSettings)
        {
            this.movableSettings = movableSettings;
        }

        public void Reset()
        {
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            Execute(movable, movableSettings);
            return true;
        }

        /// <summary>
        /// Can call this directly and externally to change settings on a movable
        /// </summary>
        /// <param name="movable">The movable that needs its settings changed</param>
        /// <param name="settings">The settings you'd like to change on the movable</param>
        public static void Execute(Movable3D movable, MovableSettings settings)
        {
            movable.Model = settings.Model;
            movable.MeshProperties = settings.MeshProperties;

            var objType = movable.GetType();

            foreach (var kvp in settings.Properties)
            {
                if (kvp.Key.ToUpper() == "ALPHA")
                {
                    movable.SetAlpha((float)kvp.Value);
                }
                else
                {
                    PropertyInfo propertyInfo = objType.GetProperty(kvp.Key);
                    if(propertyInfo != null)
                    {
                        propertyInfo.SetValue(movable, Convert.ChangeType(kvp.Value, propertyInfo.PropertyType), null);
                    }
                }
            }
        }
    }
}
