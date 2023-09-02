using Backbone.Actions;

namespace Backbone.Graphics
{
    public interface IInteractive : ICollidable, IDrawable, IActionable, IMovable
    {
        public string Name { get; set; }
    }
}
