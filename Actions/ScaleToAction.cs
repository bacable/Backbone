using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal class ScaleToAction : IAction3D
    {
        private Vector3 target;
        private float duration;
        private Vector3 source = Vector3.Zero;
        private Boolean hasStarted = false;
        private float elapsedTime = 0f;

        public ScaleToAction(Vector3 target, float duration)
        {
            this.target = target;
            this.duration = duration;
        }

        public List<Action3D> SubActions { get; set; } = new List<Action3D>();
        List<IAction3D> IAction3D.SubActions { get; set; }

        public void Reset()
        {
            hasStarted = false;
            source = Vector3.Zero;
            elapsedTime = 0f;

            if (SubActions != null)
            {
                SubActions.ForEach(x => x.Reset());
            }

        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            var isFinished = false;

            Vector3 currentVector;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += elapsed;

            if (!hasStarted)
            {
                hasStarted = true;
                source = movable.Scale;
            }
            currentVector = ActionMath.LerpVector(elapsedTime, source, target, duration);

            movable.SetScale(currentVector);

            if (elapsedTime > duration)
            {
                isFinished = true;
                Reset();
            }

            return isFinished;
        }
    }
}
