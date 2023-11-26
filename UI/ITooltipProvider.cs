using Microsoft.Xna.Framework;

namespace ProximityND.Backbone.UI
{
    public interface ITooltipProvider
    {
        public string GetTooltipText(Vector2 position);
    }
}
