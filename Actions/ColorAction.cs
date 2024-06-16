using Backbone.Actions;
using Backbone.Graphics;
using Microsoft.Xna.Framework;
using SharpDX.MediaFoundation;
using System.Collections.Generic;

namespace ProximityND.Backbone.Actions
{
    // TODO: make this generic and not just based on color1 and color2 (use mesh color properties instead)
    public class ColorAction : IAction3D
    {
        Color sourceColor;
        Color targetColor;
        float duration;
        float elapsedTime = 0f;

        private List<string> meshNames = new List<string>(); //names of meshes to adjust

        public ColorAction(string source, string target, List<string> meshNames, float duration)
        {
            sourceColor = ColorHex.ConvertFromHex(source);
            targetColor = ColorHex.ConvertFromHex(target);

            this.meshNames = meshNames;
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

            float percent = ActionMath.LerpFloat(elapsedTime, 0f, 1f, duration);
            Color currentColor = Color.Lerp(sourceColor, targetColor, percent);

            string colorHexString = ColorHex.ConvertFromColor(currentColor);

            // apply new color to all meshNames
            meshNames.ForEach(mesh =>
            {
                if (mesh.Equals("Color1"))
                {
                    movable.Color1 = colorHexString;
                }
                else if (mesh.Equals("Color2"))
                {
                    movable.Color2 = colorHexString;
                }
                else
                {
                    movable.UpdateColor(mesh, colorHexString);
                }
            });

            return (elapsedTime >= duration);
        }

        public static string Execute(string sourceColor, string targetColor, float percent)
        {
            var source = ColorHex.ConvertFromHex(sourceColor);
            var target = ColorHex.ConvertFromHex(targetColor);
            var newColor = Color.Lerp(source, target, percent);
            return ColorHex.ConvertFromColor(newColor);
        }
    }
}
