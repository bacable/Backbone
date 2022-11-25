using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Graphics
{
    public static class Collision2D
    {
        public enum Collision2DAnchor
        {
            Center,
            TopLeft,
            TopRight,
            BottomRight,
            BottomLeft
        }

        public static bool IntersectRect(Vector2 point, Rectangle boundingBox, Collision2DAnchor anchor = Collision2DAnchor.Center)
        {
            Vector2 collisionRect;

            collisionRect.X = (int)((anchor == Collision2DAnchor.Center) ? boundingBox.X - boundingBox.Width / 2f :
                (anchor == Collision2DAnchor.TopLeft || anchor == Collision2DAnchor.BottomLeft) ? boundingBox.X :
                boundingBox.X + boundingBox.Width);
            collisionRect.Y = (int)((anchor == Collision2DAnchor.Center) ? boundingBox.Y - boundingBox.Height / 2f :
                (anchor == Collision2DAnchor.TopLeft || anchor == Collision2DAnchor.TopRight) ? boundingBox.Y :
                boundingBox.Y + boundingBox.Height);

            return (point.X > collisionRect.X && point.Y > collisionRect.Y &&
                point.X < collisionRect.X + boundingBox.Width &&
                point.Y < collisionRect.Y + boundingBox.Height);
        }

        /// <summary>
        /// Determine if a point intersects a circle
        /// </summary>
        /// <param name="point">Position of point to check (i.e. Mouse cursor position)</param>
        /// <param name="target">Position of object you want to check for a collision, i.e. a UI element</param>
        /// <param name="radius">Distance from the UI element where it is considered to be a collision</param>
        /// <returns>If the position is close enough to the target to be within its radius.</returns>
        public static bool IntersectCircle(Vector2 point, Vector2 target, float radius)
        {
            var p1 = Math.Pow(target.X - point.X, 2);
            var p2 = Math.Pow(target.Y - point.Y, 2);
            var distance = Math.Sqrt(p1 + p2);
            //Debug.WriteLine("distance: " + distance);
            return distance < radius;
        }
    }
}
