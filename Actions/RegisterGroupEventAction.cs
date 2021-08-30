using Backbone.Events;
using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal class RegisterGroupEventAction<T>: IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        T eventToRegister;
        int itemId;

        public RegisterGroupEventAction(T eventToRegister, int itemId)
        {
            this.eventToRegister = eventToRegister;
            this.itemId = itemId;
        }

        public void Reset()
        {
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            PubHub<T>.RegisterAction(eventToRegister, itemId);
            return true;
        }
    }
}
