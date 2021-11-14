using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Graphics
{
    /// <summary>
    /// Based on: http://rbwhitaker.wikidot.com/picking
    /// </summary>
    public class Collision3D
    {

        public static Ray CalculateRay(Vector2 mouseLocation, Matrix view, Matrix projection, Viewport viewport)
        {
            Vector3 nearPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                    mouseLocation.Y, 0.0f),
                    projection,
                    view,
                    Matrix.Identity);

            Vector3 farPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                    mouseLocation.Y, 1.0f),
                    projection,
                    view,
                    Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        public static float? IntersectDistance(BoundingSphere sphere, Vector2 mouseLocation,
            Matrix view, Matrix projection, Viewport viewport)
        {
            Ray mouseRay = CalculateRay(mouseLocation, view, projection, viewport);
            return mouseRay.Intersects(sphere);
        }

        public static bool Intersects(Vector2 mouseLocation, Model model, Matrix world,
                                      Matrix view, Matrix projection,
                                      Viewport viewport, float? overrideRadius = null)
        {
            var distance = IntersectsWithDistance(mouseLocation, model, world, view, projection, viewport,
                overrideRadius);

            if (distance != null)
            {
                return true;
            }

            return false;

        }


        public static float? IntersectsWithDistance(Vector2 mouseLocation,
            Model model, Matrix world,
            Matrix view, Matrix projection,
            Viewport viewport, float? overrideRadius = null)
        {
            for (int index = 0; index < model.Meshes.Count; index++)
            {
                BoundingSphere sphere = model.Meshes[index].BoundingSphere;
                if(overrideRadius.HasValue)
                {
                    sphere.Radius = overrideRadius.Value;
                }

                sphere = sphere.Transform(world);
                float? distance = IntersectDistance(sphere, mouseLocation, view, projection, viewport);

                return distance;
            }

            return null;
        }
    }
}
