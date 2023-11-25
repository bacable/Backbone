using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Backbone.Graphics.Particles
{
    public class ParticleEmitter : IGUI3D
    {
        private List<Particle> particles;
        private Model particleModel;
        private float particleLife;
        private float particleGravity;
        private GraphicsDevice graphicsDevice;

        public ParticleEmitter(GraphicsDevice graphics, Model particleModel, float particleLife, float particleGravity)
        {
            this.graphicsDevice = graphics;
            this.particleModel = particleModel;
            this.particleLife = particleLife;
            this.particleGravity = particleGravity;
            particles = new List<Particle>();
        }

        public void Emit(Vector3 position, Vector3 velocity, Vector3 angularVelocity, int count)
        {
            position.Y = graphicsDevice.Viewport.Height - position.Y;

            for (int i = 0; i < count; i++)
            {
                particles.Add(new Particle(particleModel, position, velocity, angularVelocity, particleLife, particleGravity));
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
