﻿using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;

namespace Backbone.UI
{
    public class TextButtonSettings<T>
    {
        public TextGroupSettings TextGroupSettings { get; internal set; }
        public InputAction AssignedInput { get; internal set; }

        public T RaisedEventOnClick { get; internal set; }
        public float LetterCollisionRadius { get; internal set; }
        public Vector3 ClickAnimationMoveBy { get; internal set; }
        public float ClickAnimationDuration { get; internal set; }

    }
}
