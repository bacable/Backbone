using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public interface IDrawable
    {
        Dictionary<string, MeshProperty> MeshProperties { get; set; }
        void Draw(Matrix view, Matrix projection);
        void SetAlpha(float alpha);
        void Update(GameTime gameTime);
    }
}
