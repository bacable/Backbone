using System.Collections.Generic;
using System.Numerics;
using Backbone.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Backbone.Graphics
{
    public struct IconGridChooserSettings
    {
        public string Id;

        /// <summary>
        /// Space between icons
        /// </summary>
        public float GapSize;

        /// <summary>
        /// How many icons each row should have
        /// </summary>
        public int IconsPerRow;

        /// <summary>
        /// How much space do icons take up (assume square size for now)
        /// </summary>
        public float IconSize;

        /// <summary>
        /// Size of the cursor
        /// </summary>
        public float CursorScale;

        /// <summary>
        /// Model for the cursor
        /// </summary>
        public Model CursorModel;

        /// <summary>
        /// Position of top left portion of the grid
        /// </summary>
        public Microsoft.Xna.Framework.Vector3 Position;

        public Dictionary<string, MeshProperty> CursorProperties;

        public float OverrideCollisionRadius;
    }
}
