using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backbone.Actions
{
    internal class SequenceAction : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }
        int currentSubAction = 0;
        public bool repeatForever { get; set; } = false;

        public SequenceAction(params IAction3D[] sequence)
        {
            SubActions = sequence.ToList();
        }

        public void Reset()
        {
            currentSubAction = 0;

            if (SubActions != null)
            {
                SubActions.ForEach(x => x.Reset());
            }
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            var isFinished = false;

            // run subaction and see if it finished
            if (SubActions[currentSubAction].Update(movable, gameTime))
            {
                currentSubAction += 1;
                if (currentSubAction >= SubActions.Count)
                {
                    if(!repeatForever)
                    {
                        isFinished = true;
                    }
                    Reset();
                }
            }

            return isFinished;
        }
    }
}
