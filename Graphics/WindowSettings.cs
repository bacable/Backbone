using Backbone.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Graphics
{
    public class WindowSettings<T>
    {
        public T Type { get; set; }
        public Rectangle Size { get; set; }
        public Vector3 Position { get; set; }

        // How much to offset the Z position so the text of the window shows up above the window (matches the window position otherwise)
        public float TextOffsetZ { get; set; }

        // Currently only supports X dimension, but eventually will support both
        public Vector2 BackPanelScale { get; internal set; }

        // When OnClick is set, how big does it detect collisions
        public float OnClickDiameter { get; internal set; }

        public TextGroupSettings Header { get; internal set; } = null;

        // TODO: Maybe combine menu and menu position, possibly other things, to its own settings object, like with textgroupsettings above
        public MenuContainer Menu { get; internal set; }
        public Vector3 MenuPosition { get; internal set; } = Vector3.Zero;
    }
}
