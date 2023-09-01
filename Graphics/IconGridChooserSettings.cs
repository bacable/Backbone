using System.Numerics;
using Microsoft.Xna.Framework;

namespace ProximityND.Backbone.Graphics
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
        /// Position of top left portion of the grid
        /// </summary>
        public Microsoft.Xna.Framework.Vector3 Position;
    }
}
