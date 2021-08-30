using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Backbone.Graphics
{
    public class ModableGraphic
    {
        public Texture2D spriteAtlas { get; set; }

        public ModableGraphic(GraphicsDevice device, string filename)
        {
            using (var fileStream = new FileStream(filename, FileMode.Open))
            {
                spriteAtlas = Texture2D.FromStream(device, fileStream);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteAtlas, new Rectangle(0, 0, 100, 100), Color.White);
        }
    }
}
