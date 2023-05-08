using Backbone.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Graphics
{
    public class Particle
    {
        readonly ParticleSettings settings;
        Vector3 position;
        float lifespanLeft;
        float lifespanAmount;
        float elapsedTime;
        Color color;
        float opacity;
        bool isFinished = false;

        public Particle(Vector3 position, ParticleSettings settings)
        {
            this.settings = settings;
            this.position = position;
            lifespanLeft = settings.Lifespan;
            lifespanAmount = 1f;
            color = settings.StartColor;
            opacity = settings.OpacityStart;
            elapsedTime = 0f;
        }

        public void Update(GameTime gameTime)
        {
            lifespanLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(lifespanLeft < 0f)
            {
                isFinished = true;
                return;
            }

            lifespanAmount = Math.Clamp(lifespanLeft / settings.Lifespan, 0, 1);
            color = Color.Lerp(settings.EndColor, settings.StartColor, lifespanAmount);
            opacity = Math.Clamp(ActionMath.LerpFloat(elapsedTime, settings.OpacityStart, settings.OpacityEnd, settings.Lifespan), 0, 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(settings.Texture, new Vector2(position.X, position.Y), null, color * opacity, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
        }
    }
}
