using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Backbone.Actions
{
    internal class MoveAction : IAction3D
    {
        public enum MoveActionType { MoveTo, MoveBy };

        public List<IAction3D> SubActions { get; set; }

        private Vector3 target;
        private Vector3 originalVector;
        private MoveActionType moveType;
        float duration;
        private float elapsedTime = 0f;
        private bool hasCalculated = false;

        public MoveAction(MoveActionType moveType, Vector3 target, float duration)
        {
            this.target = target;
            this.moveType = moveType;
            this.duration = duration;
        }

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
            Vector3 currentVector;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += elapsed;

            // do this only once, and do not reset this
            if (!hasCalculated)
            {
                originalVector = movable.Position;
                target = (moveType == MoveActionType.MoveBy) ? originalVector + target : target;
                hasCalculated = true;
            }

            currentVector = ActionMath.LerpVector(elapsedTime, originalVector, target, duration);
            movable.Position = currentVector;

            return (elapsedTime > duration);
        }
    }
}
