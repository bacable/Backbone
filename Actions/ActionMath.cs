using Microsoft.Xna.Framework;
using System;

namespace Backbone.Actions
{
    internal static class ActionMath
    {
        internal static float ParametricBlend(float t)
        {
            float sqt = t * t;
            return sqt / (2.0f * (sqt - t) + 1.0f);
        }

        internal static float LerpFloat(float elapsed, float source, float target, float duration, ActionAnimationType animType = ActionAnimationType.Parametric)
        {
            float rawPercent = animType == ActionAnimationType.Linear ? (float)(elapsed / duration) : ParametricBlend((float)(elapsed / duration));
            float clampedPercent = (duration == 0f) ? 1.0f : Math.Min(1.0f, rawPercent);
            var newFloat = source * (1 - clampedPercent) + target * clampedPercent;
            return newFloat;
        }

        internal static Vector3 LerpVector(float elapsed, Vector3 source, Vector3 target, float duration, ActionAnimationType animType = ActionAnimationType.Parametric)
        {
            float rawPercent = animType == ActionAnimationType.Linear ? (float)(elapsed / duration) : ParametricBlend((float)(elapsed / duration));
            float clampedPercent = (duration == 0f) ? 1.0f : Math.Min(1.0f, rawPercent);
            var newVector = Vector3.Lerp(source, target, clampedPercent);
            return newVector;
        }
    }
}
