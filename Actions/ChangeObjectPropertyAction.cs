using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Backbone.Actions
{
    /// <summary>
    /// Takes in an object, a string name of a property on that object, and a new value, and will change the property of that object
    /// Will immediately make that change and finish the action.
    /// Mainly used to adjust a value during an existing sequence of actions, either after a set delay, or more likely in the middle of an
    /// animation.
    /// </summary>
    public class ChangeObjectPropertyAction<T> : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        object targetObject;
        string propertyName;
        T newPropertyValue;

        public ChangeObjectPropertyAction(object targetObject, string propertyName, T newPropertyValue)
        {
            this.targetObject = targetObject;
            this.propertyName = propertyName;
            this.newPropertyValue = newPropertyValue;
        }

        public void Reset()
        {
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            var objType = targetObject.GetType();
            PropertyInfo propertyInfo = objType.GetProperty(propertyName);
            propertyInfo.SetValue(targetObject, Convert.ChangeType(newPropertyValue, propertyInfo.PropertyType), null);
            return true;
        }
    }
}
