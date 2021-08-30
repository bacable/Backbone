using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backbone.Actions
{
    internal class GroupAction : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        public GroupAction(params IAction3D[] groupActions)
        {
            SubActions = groupActions.ToList();
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
            var isFinished = false;
            if (UpdateGroup(movable, gameTime))
            {
                isFinished = true;
                Reset();
            }
            return isFinished;
        }

        // TODO: this looks like actions could be executed a bunch of times if the whole group doesn't end at once
        private bool UpdateGroup(Movable3D movable, GameTime gameTime)
        {
            bool isGroupFinished = true;
            foreach (var action in SubActions)
            {
                if (!action.Update(movable, gameTime))
                {
                    isGroupFinished = false;
                }
            }
            return isGroupFinished;
        }
    }
}
