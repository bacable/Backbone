using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Graphics
{
    /// <summary>
    /// Based on: http://rbwhitaker.wikidot.com/picking
    /// Note: Having some issues getting this to work consistently between models. It was working for my hex tiles fine,
    /// but have had issues with it for the text and icons. Not sure if it's the way the models
    /// are being output (they seem the same, same size, same origin, etc, or what. Anyway, my game is basically 2D anyway,
    /// so I will circle back to this sometime later to make sure it's working correctly. Just keep in mind it might not be
    /// without some errors.
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

        public static bool IntersectsSphere(Vector2 mouseLocation,
            BoundingSphere sphere, Matrix world,
            Matrix view, Matrix projection,
            Viewport viewport)
        {
            sphere = sphere.Transform(world);
            return (IntersectDistance(sphere, mouseLocation, view, projection, viewport) != null);
        }

        public static bool HasSphereCollision(HandleMouseCommand command, Movable3D obj, float collisionRadius)
        {
            return Collision3D.Intersects(command.MousePosition, obj.Model, obj.World, command.View, command.Projection, command.Viewport, collisionRadius);
        }

        /// <summary>
        /// Checks a list of models as a group with a larger bounding sphere encapsulating all of them
        /// (for example, the TextGroup, as a series of letters). This allows us to test the large sphere once to see
        /// if we're in the right region, and if that intersects, then check each smaller element (letter in the word) to determine
        /// more accurately if there was a collision
        /// </summary>
        /// <param name="command"></param>
        /// <param name="smallObjs">list of models to do the smaller test on (i.e.</param>
        /// <param name="smallCollisionRadius">collision radius for each smaller element</param>
        /// <param name="groupWorld">world object for the group model (likely a parent node)</param>
        /// <param name="groupSphere">bounding sphere for everything</param>
        /// <returns></returns>
        public static bool HasSphereCollision(HandleMouseCommand command, List<Movable3D> smallObjs, float smallCollisionRadius, Matrix groupWorld, BoundingSphere groupSphere)
        {
            // check larger circle, then if that matches, check each individual letter to get more accurate collision

            //TODO: intersectssphere isn't working right now. get it working and then remove the 'true'. letter intersections work fine though
            if(true || IntersectsSphere(command.MousePosition, groupSphere, groupWorld, command.View, command.Projection, command.Viewport))
            {
                for(var i=0; i<smallObjs.Count; i++)
                {
                    var obj = smallObjs[i];
                    if(Intersects(command.MousePosition, obj.Model, obj.World, command.View, command.Projection, command.Viewport, smallCollisionRadius))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
