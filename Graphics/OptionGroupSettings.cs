using Backbone.Actions;
using Backbone.Graphics;
using Backbone.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ProximityND.Backbone.Graphics
{
    public class OptionGroupSettings
    {
        public Movable3D ParentMovable { get; set; } = null;
        public Vector3 Position { get; set; } = Vector3.Zero;
        public MenuContainer Menu { get; set; } = null;
        public List<Func<int, IAction3D>> TransitionInAnims { get; set; } = null;
        public List<Func<int, IAction3D>> TransitionOutAnims { get; set; } = null;
    }
}
