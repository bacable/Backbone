using Backbone.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Graphics
{
    public class ModelDrawSettings
    {
        public Model Model { get; set; }
        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public float Alpha { get; set; }
        public Dictionary<string, MeshProperty> MeshProperties { get; set; } = new Dictionary<string, MeshProperty>();
    }
}
