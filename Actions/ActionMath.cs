using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal static class ActionMath
    {
        internal static float ParametricBlend(float t)
        {
            float sqt = t * t;
            return sqt / (2.0f * (sqt - t) + 1.0f);
        }

        internal static float LerpFloat(float elapsed, float source, float target, float duration)
        {
            float percent = (duration == 0f) ? 1.0f : Math.Min(1.0f, ParametricBlend((float)(elapsed / duration)));
            var newFloat = source * (1 - percent) + target * percent;
            return newFloat;
        }


        internal static Vector3 LerpVector(float elapsed, Vector3 source, Vector3 target, float duration)
        {
            float percent = (duration == 0f) ? 1.0f : Math.Min(1.0f, ParametricBlend((float)(elapsed / duration)));
            var newVector = Vector3.Lerp(source, target, percent);
            return newVector;
        }
    }
}
