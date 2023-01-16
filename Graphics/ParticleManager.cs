using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Graphics
{
    public static class ParticleManager
    {
        static readonly List<Particle> particles = new();

        public static void Add(Particle particle)
        {
            particles.Add(particle);
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var particle in particles)
            {
                particle.Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(var particle in particles)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}
