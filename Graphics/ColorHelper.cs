using Backbone.Input;

namespace Backbone.Graphics
{
    public static class ColorHelper
    {
        public static ColorType FromName(string colorTypeName)
        {
            return EnumHelper<ColorType>.FromString(colorTypeName);
        }
    }
}
