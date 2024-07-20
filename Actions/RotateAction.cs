using Backbone.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal class RotateAction : IAction3D
    {
        public enum Coordinate { X, Y, Z }
        public List<IAction3D> SubActions { get; set; }

        public bool RepeatForever { get; set; } = false;

        private float target;
        private float duration;
        private Coordinate coordinate;

        private bool hasStarted = false;
        private float source = 0f;
        private float elapsedTime = 0f;

        public RotateAction(Coordinate coordinate, float rotateAmount, float duration)
        {
            this.target = rotateAmount;
            this.duration = duration;
            this.coordinate = coordinate;
        }

        public void Reset()
        {
            hasStarted = false;
            source = 0f;
            elapsedTime = 0f;
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            float currentFloat;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += elapsed;

            if (!hasStarted)
            {
                hasStarted = true;
                switch(coordinate)
                {
                    case Coordinate.X:
                        source = movable.RotationX;
                        break;
                    case Coordinate.Y:
                        source = movable.RotationY;
                        break;
                    case Coordinate.Z:
                        source = movable.RotationZ;
                        break;
                }
            }

            currentFloat = ActionMath.LerpFloat(elapsedTime, source, target, duration);
            
            switch(coordinate)
            {
                case Coordinate.X:
                    movable.RotationX = currentFloat;
                    break;
                case Coordinate.Y:
                    movable.RotationY = currentFloat;
                    break;
                case Coordinate.Z:
                    movable.RotationZ = currentFloat;
                    break;
            }

            if (elapsedTime >= duration)
            {
                if(RepeatForever)
                {
                    elapsedTime -= duration;
                } else
                {
                    return true;
                }
            }

            return false;
        }

    }
}
