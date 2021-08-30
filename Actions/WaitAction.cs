using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal class WaitAction : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }
        private float target;
        private float elapsedTime = 0f;

        public WaitAction(float duration)
        {
            this.target = duration;
        }

        public void Reset()
        {
            elapsedTime = 0f;
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            bool isFinished = false;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            elapsedTime += elapsed;

            if (elapsedTime > target)
            {
                isFinished = true;
                Reset();
            }

            return isFinished;
        }
    }
}
