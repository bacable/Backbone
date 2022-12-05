using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    public class FadeToAction : IAction3D
    {
        private float target;
        private float source;
        private float duration;
        private Boolean hasStarted = false;
        private float elapsedTime = 0f;
        
        public FadeToAction(float target, float duration)
        {
            this.target = target;
            this.duration = duration;
        }

        public List<IAction3D> SubActions { get; set; } = new List<IAction3D>();

        public void Reset()
        {
            hasStarted = false;
            source = 0f;
            elapsedTime = 0f;

            if(SubActions != null)
            {
                SubActions.ForEach(x => x.Reset());
            }
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            var isFinished = false;

            float currentFloat;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += elapsed;

            if (!hasStarted)
            {
                hasStarted = true;
                source = movable.Alpha;
            }
            currentFloat = ActionMath.LerpFloat(elapsedTime, source, target, duration);

            movable.Alpha = currentFloat;

            if(elapsedTime > duration)
            {
                isFinished = true;
                Reset();
            }

            return isFinished;
        }
    }
}
