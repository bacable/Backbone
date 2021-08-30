using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    public interface IAction3D
    {
        List<IAction3D> SubActions { get; set; }
        void Reset();
        bool Update(Movable3D movable, GameTime gameTime);
    }
}
