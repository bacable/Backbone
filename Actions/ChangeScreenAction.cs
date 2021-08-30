using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal class ChangeScreenAction<T> : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        T switchToScreen;

        public ChangeScreenAction(T switchToScreen)
        {
            this.switchToScreen = switchToScreen;
        }

        public void Reset()
        {

        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            ScreenManager<T>.SwitchTo(switchToScreen);
            return true;
        }
    }
}
