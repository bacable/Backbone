using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProximityND.Backbone.Actions;
using System;
using System.Linq;

namespace Backbone.Actions
{
    // TODO: Add run local code block (passed as lambda) if possible
    public class ActionBuilder
    {
        public static IAction3D Scale(Vector3 source, Vector3 target, float duration)
        {
            return new ScaleToAction(source, target, duration);
        }

        internal static IAction3D FadeTo(float target, float duration)
        {
            return new FadeToAction(target, duration);
        }

        public static IAction3D RotateX(float target, float duration)
        {
            return new RotateAction(RotateAction.Coordinate.X, target, duration);
        }
        public static IAction3D RotateY(float target, float duration)
        {
            return new RotateAction(RotateAction.Coordinate.Y, target, duration);
        }

        public static IAction3D RotateZ(float target, float duration)
        {
            return new RotateAction(RotateAction.Coordinate.Z, target, duration);
        }

        public static IAction3D MoveBy(Vector3 by, float duration)
        {
            return new MoveAction(MoveAction.MoveActionType.MoveBy, by, duration);
        }

        public static IAction3D MoveTo(Vector3 target, float duration)
        {
            return new MoveAction(MoveAction.MoveActionType.MoveTo, target, duration);
        }

        public static IAction3D Wait(float duration)
        {
            return new WaitAction(duration);
        }

        public static IAction3D RegisterGroupEvent<T>(T eventToRegister, int itemId)
        {
            return new RegisterGroupEventAction<T>(eventToRegister, itemId);
        }

        public static IAction3D RaiseGroupEvent<T>(T groupEventToRaise, int itemId, object payload)
        {
            return new RaiseGroupEventAction<T>(groupEventToRaise, itemId, payload);
        }

        public static IAction3D RaiseEvent<T>(T eventToRaise, object payload)
        {
            return new RaiseEventAction<T>(eventToRaise, payload);
        }

        public static IAction3D PlaySound<T>(T soundToPlay)
        {
            return new PlaySoundAction<T>(soundToPlay);
        }

        public static void AddToGroup(ref IAction3D action, params IAction3D[] groupActions)
        {
            if(action is GroupAction)
            {
                action.SubActions.AddRange(groupActions);
            }
        }

        public static IAction3D Group(params IAction3D[] groupActions)
        {
            return new GroupAction(groupActions);
        }

        public static IAction3D Sequence(params IAction3D[] sequence)
        {
            return new SequenceAction(sequence);
        }

        public static IAction3D ChangeModel(Model newModel)
        {
            return new ChangeModelAction(newModel);
        }

        public static IAction3D ChangeScreen<T>(T switchToScreen)
        {
            return new ChangeScreenAction<T>(switchToScreen);
        }

        /// <summary>
        /// Takes in an object, a string name of a property on that object, and a new value, and will change the property of that object
        /// Will immediately make that change and finish the action.
        /// Mainly used to adjust a value during an existing sequence of actions, either after a set delay, or more likely in the middle of an
        /// animation.
        /// </summary>
        /// <typeparam name="T">The type for the property value.</typeparam>
        /// <param name="targetObject">The object that has the property we want to change.</param>
        /// <param name="propertyName">The name of the property on the object we want to change.</param>
        /// <param name="newPropertyValue">The new value we want that property to become.</param>
        /// <returns></returns>
        public static IAction3D ChangeObjectProperty<T>(object targetObject, string propertyName, T newPropertyValue)
        {
            return new ChangeObjectPropertyAction<T>(targetObject, propertyName, newPropertyValue);
        }

        public static IAction3D AddVelocity(Vector3 v0, Vector3 p0, float g, float duration)
        {
            return new PhysicsParticleAction(v0, p0, g, duration);
        }

        public static IAction3D Rumble(PlayerIndex playerIndex, float leftMotor, float rightMotor, float duration)
        {
            return new RumbleAction(playerIndex, leftMotor, rightMotor, duration);
        }

        internal static IAction3D ChangeColor(Color startColor, Color endColor, float duration)
        {
            return new ColorAction(startColor, endColor, duration);
        }

        /// <summary>
        /// Reposition an object immediately, no lerping or updates or duration. Had too many issues with 
        /// the normal move action with a tiny duration to work like this, figured it's best just to add this and make it simple
        /// </summary>
        /// <param name="position">The new position to update the movable to.</param>
        /// <returns>The reposition action.</returns>
        internal static IAction3D Reposition(Vector3 position)
        {
            return new RepositionAction(position);
        }
    }
}
