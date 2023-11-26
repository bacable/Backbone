using Backbone.Actions;
using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Backbone.Actions.RotateAction;

namespace ProximityND.Backbone.Actions
{
    public class ColorAction : IAction3D
    {
        Color sourceColor;
        Color targetColor;
        float duration;
        float elapsedTime = 0f;

        public ColorAction(string source, string target, float duration)
        {
            sourceColor = ColorHex.ConvertFromHex(source);
            targetColor = ColorHex.ConvertFromHex(target);
            this.duration = duration;
        }

        public ColorAction(Color source, Color target, float duration)
        {
            sourceColor = source;
            targetColor = target;
            this.duration = duration;
        }

        public List<IAction3D> SubActions { get; set; }

        public void Reset()
        {
            elapsedTime = 0f;

            if (SubActions != null)
            {
                SubActions.ForEach(x => x.Reset());
            }
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += elapsed;

            float percent = ActionMath.LerpFloat(elapsed, 0f, 1f, duration);
            Color currentColor = Color.Lerp(sourceColor, targetColor, percent);

            string colorHexString = ColorHex.ConvertFromColor(currentColor);

            movable.Color1 = colorHexString;
            movable.Color2 = colorHexString;

            return (elapsedTime >= duration);
        }
    }
}
