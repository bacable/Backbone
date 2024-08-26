using Backbone.Actions;
using Backbone.Graphics;
using Backbone.Input;
using Backbone.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public class OptionGroupSettings
    {
        public Movable3D ParentMovable { get; set; } = null;
        public Vector3 Position { get; set; } = Vector3.Zero;
        public MenuContainer Menu { get; set; } = null;
        public List<Func<int, IAction3D>> TransitionInAnims { get; set; } = null;
        public List<Func<int, IAction3D>> TransitionOutAnims { get; set; } = null;

        public IGUI3D LeftArrowButton { get; set; } = null;
        public IGUI3D RightArrowButton { get; set; } = null;

        public string SelectedColor { get; set; } = "#FFFFFF";
        public string UnselectedColor { get; set; } = "#AAAAAA";
        public bool UpdateAfterClick { get; set; } = true;
        public MouseEvent LeftClickType { get; set; } = MouseEvent.LeftButtonReleased;
        public MouseEvent RightClickType { get; set; } = MouseEvent.RightButtonReleased;
    }
}
