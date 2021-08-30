using Backbone.Events;
using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal class RaiseGroupEventAction<T> : IAction3D
    {
        private T groupEventToRaise;
        private int itemId;
        private object payload;

        public RaiseGroupEventAction(T groupEventToRaise, int itemId, object payload)
        {
            this.groupEventToRaise = groupEventToRaise;
            this.itemId = itemId;
            this.payload = payload;
        }

        public List<IAction3D> SubActions { get; set; }

        public void Reset()
        {
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            PubHub<T>.FinishedAction(groupEventToRaise, itemId, payload);
            return true;
        }
    }
}
