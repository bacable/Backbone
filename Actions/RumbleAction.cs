using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
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
        private string category;
        public RumbleAction(PlayerIndex playerIndex, float leftMotor, float rightMotor, float duration, string category)
        {
            this.playerIndex = playerIndex;
            this.leftMotor = leftMotor;
            this.rightMotor = rightMotor;
            this.duration = duration;
            this.category = category;
        }

        public void Reset()
        {
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
            RumbleManager.Rumble(playerIndex, leftMotor, rightMotor, duration, category);
            return true;
        }
    }
}
