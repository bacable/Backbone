using Backbone.Actions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Graphics
{
    public class TextGroupSettings
    {
        public int Id { get; internal set; } = 0;
        public ColorType Color { get; internal set; }
        public Movable3D Parent { get; set; }
        public Vector3 Position { get; internal set; }
        public float Scale { get; internal set; }
        public string Text { get; internal set; }
        public Func<int, IAction3D> TransitionInAnim { get; set; } = null;
        public Func<int, IAction3D> TransitionOutAnim { get; set; } = null;
    }
}
