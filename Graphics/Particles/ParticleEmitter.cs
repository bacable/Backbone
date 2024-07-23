using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Backbone.Graphics.Particles
{
    public class ParticleEmitter : IGUI3D
    {
        private List<Particle> particles;
        private Model particleModel;
        private float particleLife;
        private float particleGravity;
        private float particleScale;
        private bool shouldInvertY;
        private GraphicsDevice graphicsDevice;

        public ParticleEmitter(GraphicsDevice graphics, Model particleModel, float particleLife, float particleGravity, float particleScale, bool shouldInvertY = false)
        {
            this.graphicsDevice = graphics;
            this.particleModel = particleModel;
            this.particleLife = particleLife;
            this.particleGravity = particleGravity;
            this.particleScale = particleScale;
            this.shouldInvertY = shouldInvertY;

            particles = new List<Particle>();
        }

        public void Emit(Vector3 position, Vector3 velocity, Vector3 angularVelocity, int count)
        {
            if(shouldInvertY)
            {
                position.Y = graphicsDevice.Viewport.Height - position.Y;
            }

            for (int i = 0; i < count; i++)
            {
                particles.Add(new Particle(particleModel, position, velocity, angularVelocity, particleLife, particleGravity, particleScale));
            }
        }

        public void Emit(Vector3 position, Func<Vector3> velocityFunction, Vector3 angularVelocity, int count)
        {
            if (shouldInvertY)
            {
                position.Y = graphicsDevice.Viewport.Height - position.Y;
            }

            for (int i = 0; i < count; i++)
            {
                var velocity = velocityFunction.Invoke();
                particles.Add(new Particle(particleModel, position, velocity, angularVelocity, particleLife, particleGravity, particleScale));
            }
        }


        public void Update(GameTime gameTime)
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(gameTime);

                // Remove dead particles
                if (particles[i].Life <= 0)
                {
                    particles.RemoveAt(i);
                }
            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (var particle in particles)
            {
                // Draw each particle
                foreach (var mesh in particle.Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = particle.World;
                        effect.View = view;
                        effect.Projection = projection;
                        effect.Alpha = particle.Alpha;
                    }
                    mesh.Draw();
                }
            }
        }

        public void HandleMouse(HandleMouseCommand command)
        {
        }

        public void TransitionIn()
        {
            // shouldn't ever need anything here, won't be anything onscreen to transition
        }

        public void TransitionOut()
        {
            // might need something here if still on the screen
        }
    }
}
