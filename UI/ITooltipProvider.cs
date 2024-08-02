using Microsoft.Xna.Framework;

namespace Backbone.UI
{
    public interface ITooltipProvider
    {
        public string GetTooltipText(Vector2 position);
    }
}
