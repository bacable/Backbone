using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Graphics
{
    public interface ICollidable
    {
        bool Intersects(Viewport viewport, Vector2 point, Vector2 target, float? overrideRadius = null);
    }
}
