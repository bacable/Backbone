using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.UI
{
    public class TextButtonSettings<T>
    {
        public TextGroupSettings TextGroupSettings { get; set; }
        public InputAction AssignedKey { get; set; }

        public T RaisedEventOnClick { get; set; }

    }
}
