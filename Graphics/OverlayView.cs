using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Backbone.Graphics
{
    public class OverlayView
    {
        Texture2D pixel;
        private GraphicsDevice device;
        private float alpha = 0.9f;
        private string overlayColorString = "#000000";
        private Color overlayColor = ColorHex.ConvertFromHex("#000000", 0.9f);

        public OverlayView(GraphicsDevice device)
        {
            this.device = device;
            pixel = new Texture2D(device, 1, 1);
            pixel.SetData(new[] {  Color.White });
        }

        private void SetColor(string newColor)
        {
            overlayColor = ColorHex.ConvertFromHex(newColor, alpha);
            overlayColorString = newColor;
        }

        private void UpdateAlpha(float newAlpha)
        {
            alpha = newAlpha;
            overlayColor = ColorHex.ConvertFromHex(overlayColorString, alpha);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(pixel,
                new Rectangle(0, 0, device.Viewport.Width, device.Viewport.Height),
                overlayColor);
            spriteBatch.End();
        }
    }
}
