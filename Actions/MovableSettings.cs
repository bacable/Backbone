using Backbone.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Backbone.Actions
{
    public class MovableSettings
    {
        public Model Model { get; set; }
        public Dictionary<string, MeshProperty> MeshProperties { get; set; } = new Dictionary<string, MeshProperty>();
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public bool IsOccupied { get; set; }
    }
}
