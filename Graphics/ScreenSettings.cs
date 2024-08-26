using Microsoft.Xna.Framework;

namespace Backbone.Graphics
{
    public class ScreenSettings
    {
        const int DEFAULT_SCREEN_WIDTH = 1920;
        const int DEFAULT_SCREEN_HEIGHT = 1080;

        public static GraphicsDeviceManager Graphics { get; set; } = null;
        public static int Width
        {
            get
            {
                return Graphics != null ? Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width : DEFAULT_SCREEN_WIDTH;
            }
        }

        public static int Height
        {
            get
            {
                return Graphics != null ? Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height : DEFAULT_SCREEN_HEIGHT;
            }
        }

        public static int BaseHeight
        {
            get
            {
                return DEFAULT_SCREEN_HEIGHT;
            }
        }

        public static int BaseWidth
        {
            get
            {
                return DEFAULT_SCREEN_WIDTH;
            }
        }

        public static int ResolutionWidth
        {
            get; set;
        } = DEFAULT_SCREEN_WIDTH;

        public static int ResolutionHeight
        {
            get; set;
        } = DEFAULT_SCREEN_HEIGHT;

        public static float HorizontalRatio
        {
            get
            {
                return Graphics != null ? Graphics.GraphicsDevice.Viewport.Width / (float)DEFAULT_SCREEN_WIDTH : 1.0f;
            }
        }
        public static float VerticalRatio
        {
            get
            {
                return Graphics != null ? Graphics.GraphicsDevice.Viewport.Height / (float)DEFAULT_SCREEN_HEIGHT : 1.0f;
            }
        }

    }
}
