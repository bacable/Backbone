using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Graphics
{
    public class ParticleSettings
    {
//        private static Texture2D defaultParticle;
        public Texture2D Texture { get; set; }
        public float Lifespan { get; set; }
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public float OpacityStart { get; set; }
        public float OpacityEnd { get; set; }
    }
}
