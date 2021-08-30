using Backbone.Events;
using Backbone.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Backbone.Actions
{
    public class Action3D : IAction3D
    {
        public static Dictionary<ActionType, Type> Template { get; set; } = new Dictionary<ActionType, Type>();

        public int Id { get; set; }
        public Model TargetModel { get; set; }
        public Vector3 TargetVector { get; set; }
        public ActionType Action { get; set; }
        public string TargetScreen { get; set; }
        public float TargetFloat { get; set; }
        public float Duration { get; set; }

        /// <summary>
        /// The object that gets passed when an event is raised
        /// </summary>
        public object Payload { get; set; }

        public Type GameEventEndToRaise { get; set; }

        public Type MenuEventToRaise { get; set; }

        public Type EffectToPlay { get; set; }

        public List<IAction3D> SubActions { get; set; } = new List<IAction3D>();
        public bool RepeatForever { get; set; } = false;

        private Boolean hasStarted = false;
        private Vector3 originalVector = Vector3.Zero;
        private float originalFloat = 0f;
        private float elapsedTime = 0f;
        private int currentSubAction = 0;

        public Action3D()
        {
        }

        public void Reset()
        {
            hasStarted = false;
            originalVector = Vector3.Zero;
            originalFloat = 0f;
            elapsedTime = 0f;
            currentSubAction = 0;

            if(SubActions != null)
            {
                SubActions.ForEach(x => x.Reset());
            }
        }

        private float ParametricBlend(float t)
        {
            float sqt = t * t;
            return sqt / (2.0f * (sqt - t) + 1.0f);
        }

        private Vector3 UpdateVector(float elapsed)
        {
            elapsedTime += elapsed;
            float percent = (Duration == 0f) ? 1.0f : Math.Min(1.0f, ParametricBlend((float)(elapsedTime / Duration)));
            var newVector = Vector3.Lerp(originalVector, TargetVector, percent);
            return newVector;
        }

        private float UpdateFloat(float elapsed)
        {
            elapsedTime += elapsed;
            float percent = (Duration == 0f) ? 1.0f : Math.Min(1.0f, ParametricBlend((float)(elapsedTime / Duration)));
            var newFloat = Lerp(originalFloat, TargetFloat, percent);
            return newFloat;
        }

        private float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        private bool UpdateGroup(Movable3D movable, GameTime gameTime)
        {
            bool isGroupFinished = true;
            foreach(var action in SubActions)
            {
                if(!action.Update(movable, gameTime))
                {
                    isGroupFinished = false;
                }
            }
            return isGroupFinished;
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            var isFinished = false;
            Vector3 currentVector;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch(Action)
            {
                case ActionType.MoveBy:
                    if (!hasStarted)
                    {
                        hasStarted = true;
                        originalVector = movable.Position;
                        TargetVector = originalVector + TargetVector;
                    }
                    currentVector = UpdateVector(elapsed);
                    movable.Position = currentVector;
                    break;
                case ActionType.MoveTo:
                    if(!hasStarted)
                    {
                        hasStarted = true;
                        originalVector = movable.Position;
                    }
                    currentVector = UpdateVector(elapsed);
                    movable.Position = currentVector;
                    break;
                case ActionType.Group:
                    if(UpdateGroup(movable, gameTime))
                    {
                        isFinished = true;
                        Reset();
                    }
                    break;
                case ActionType.Sequence:
                    // run subaction and see if it finished
                    if(SubActions[currentSubAction].Update(movable, gameTime))
                    {
                        currentSubAction += 1;
                        if(currentSubAction >= SubActions.Count)
                        {
                            isFinished = true;
                            Reset();
                        }
                    }
                    break;
                default:
                    isFinished = true;
                    break;
            }

            if(Action != ActionType.Group && Action != ActionType.Sequence && elapsedTime > Duration)
            {
                isFinished = true;
                Reset();
            }

            if(isFinished && RepeatForever)
            {
                isFinished = false;
            }

            return isFinished;

        }
    }
}
