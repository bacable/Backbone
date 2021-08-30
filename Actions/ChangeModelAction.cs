using Backbone.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal class ChangeModelAction : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        Model changeTo;

        public ChangeModelAction(Model changeTo)
        {
            this.changeTo = changeTo;
        }

        public void Reset()
        {
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            movable.Model = changeTo;
            return true;
        }
    }
}
