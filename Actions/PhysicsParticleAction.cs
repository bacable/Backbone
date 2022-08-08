using Backbone.Actions;
using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Backbone.Actions
{
    public class PhysicsParticleAction : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        // initial position and velocity
        private Vector3 v0;
        private Vector3 p0;

        private Vector3 vCurrent;
        private Vector3 pCurrent;

        private float g; // how much gravitational force

        private float elapsedTime = 0;
        private float duration;

        public PhysicsParticleAction(Vector3 v0, Vector3 p0, float g, float duration)
        {
            this.v0 = v0;
            this.p0 = p0;
            this.g = g;
            this.duration = duration;

            vCurrent = v0;
            pCurrent = p0;
        }

        public void Reset()
        {
            elapsedTime = 0f;

            vCurrent = v0;
            pCurrent = p0;

            if (SubActions != null)
            {
                SubActions.ForEach(x => x.Reset());
            }
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += elapsed;

            vCurrent = new Vector3(vCurrent.X, vCurrent.Y - g * elapsed, vCurrent.Z);
            pCurrent = new Vector3(pCurrent.X + elapsed * vCurrent.X, pCurrent.Y + elapsed * vCurrent.Y, pCurrent.Z + elapsed * vCurrent.Z);

            movable.Position = pCurrent;

            return (elapsedTime > duration);
        }
    }
}
