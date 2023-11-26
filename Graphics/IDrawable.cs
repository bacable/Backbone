using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public interface IDrawable
    {
        Dictionary<string, MeshProperty> MeshProperties { get; set; }
        void Draw(Matrix view, Matrix projection);
        void Update(GameTime gameTime);
    }
}
