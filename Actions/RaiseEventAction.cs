using Backbone.Events;
using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal class RaiseEventAction<T> : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        private T eventToRaise;
        object payload;

        public RaiseEventAction(T eventToRaise, object payload)
        {
            this.eventToRaise = eventToRaise;
            this.payload = payload;
        }

        public void Reset()
        {
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            PubHub<T>.Raise(eventToRaise, payload);
            return true;
        }
    }
}
