using Backbone.Actions;
using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Actions
{
    public class RepositionAction : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        private Vector3 newPosition { get; set; }

        public RepositionAction(Vector3 newPosition)
        {
            this.newPosition = newPosition;
        }

        public void Reset()
        {
            if (SubActions != null)
            {
                SubActions.ForEach(x => x.Reset());
            }
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            movable.UpdatePosition(newPosition);
            return true;
        }
    }
}
