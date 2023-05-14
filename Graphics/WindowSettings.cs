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

        // Where the window is located while it is active
        public Vector3 ActivePosition { get; set; }

        // Where the window is located while it is inactive (usually an offscreen position)
        public Vector3 InactivePosition { get; set; }

        // How long it takes the window to move from inactive/active positions. Default is half a second.
        public float TransitionDuration { get; set; } = 0.5f;

        // How much to offset the Z position so the text of the window shows up above the window (matches the window position otherwise)
        public float TextOffsetZ { get; set; }

        // Currently only supports X dimension, but eventually will support both
        public Vector2 BackPanelScale { get; internal set; }

        // When OnClick is set, how big does it detect collisions
        public float OnClickDiameter { get; internal set; }

        public TextGroupSettings Header { get; internal set; } = null;

        // TODO: Maybe combine menu and menu position, possibly other things, to its own settings object, like with textgroupsettings above
        public MenuContainer Menu { get; internal set; }

        // Some windows want to draw/update even while inactive, but by default we don't want to processing on drawing if not active
        public bool VisibleWhileInactive { get; internal set; } = false;

        // The position the options menu is located in relation to the backpanel of the window
        public Vector3 MenuPosition { get; internal set; } = Vector3.Zero;

        // Show close button
        public bool ShowCornerCloseButton { get; internal set; } = false;

        // Event that fires when close button clicked
        public Action CloseButtonAction { get; internal set; } = null;

        public string SelectedColor { get; internal set; } = "#FFFFFF";
        public string UnselectedColor { get; internal set; } = "#AAAAAA";
    }
}
