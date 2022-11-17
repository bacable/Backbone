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
        bool Intersects(Vector2 position, Matrix view, Matrix projection, Viewport viewport, float? overrideSphereRadius = null);
    }
}
