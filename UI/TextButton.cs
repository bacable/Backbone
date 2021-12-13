using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.UI
{
    public class TextButton : IGUI3D
    {
        private TextGroup Text;

        public TextButton(TextButtonSettings settings)
        {
            Text = new TextGroup(settings);
        }

        public void HandleMouse(HandleMouseCommand command)
        {
        }

        public void TransitionIn()
        {
        }

        public void TransitionOut()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(Matrix view, Matrix projection)
        {
        }

    }
}
