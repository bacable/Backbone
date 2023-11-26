using Microsoft.Xna.Framework;

namespace Backbone.Graphics
{
    public interface IMovable
    {
        Vector3 GetPosition();
        void UpdatePosition(Vector3 newPosition);
    }
}
