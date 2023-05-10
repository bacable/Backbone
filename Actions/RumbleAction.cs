using Backbone.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Backbone.Actions
{

    public class RumbleAction : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        private PlayerIndex playerIndex;
        private float leftMotor;
        private float rightMotor;
        private float duration;
        private float elapsedTime;
        private bool hasStarted;
        public RumbleAction(PlayerIndex playerIndex, float leftMotor, float rightMotor, float duration)
        {
            this.playerIndex = playerIndex;
            this.leftMotor = leftMotor;
            this.rightMotor = rightMotor;
            this.duration = duration;
            this.elapsedTime = 0f;
            this.hasStarted = false;
        }

        public void Reset()
        {
            elapsedTime = 0f;
            hasStarted = false;

            if (SubActions != null)
            {
                SubActions.ForEach(x =>
                {
                    x.Reset();
                });
            }
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += elapsed;

            if (!hasStarted)
            {
                GamePad.SetVibration(playerIndex, leftMotor, rightMotor);
                hasStarted = true;
            }

            if (elapsedTime > duration)
            {
                GamePad.SetVibration(playerIndex, 0f, 0f);
                return true;
            }

            return false;
        }
    }
}
