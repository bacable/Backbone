﻿using Backbone.Actions;
using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
using System;

namespace Backbone.UI
{
    public class TextButton<T> : IGUI3D
    {
        private TextGroup Text;

        TextButtonSettings<T> settings;

        bool hasClicked = false;
        bool checkInput = true;

        float resetHasClickedSeconds = 0.0f;

        private bool enabled = true;
        public bool Enabled { get
            {
                return enabled;
            } set
            {
                if(enabled != value)
                {
                    Text.SetAlpha(value ? 1.0f : 0.4f, 0.4f);
                    enabled = value;
                }
            } }

        public TextButton(TextButtonSettings<T> settings)
        {
            this.settings = settings;
            
            Text = new TextGroup(settings.TextGroupSettings);
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            if(command.State == MouseEvent.LeftButtonPressed)
            {
                if (Collision3D.HasSphereCollision(command, Text.Letters, settings.LetterCollisionRadius, Text.World.Value, Text.GroupBoundingSphere))
                {
                    hasClicked = true;
                    resetHasClickedSeconds = settings.ClickAnimationDuration;
                }
            }
        }

        public void TransitionIn()
        {
            Text.TransitionIn();
        }

        public void TransitionOut()
        {
            Text.TransitionOut();
        }

        public void SetColor(string hexCode)
        {
            Text.SetColor(hexCode);
        }

        public void Update(GameTime gameTime)
        {
            Text.Update(gameTime);

            if (Text.IsAnimating) { return; }

            var keyPressed = InputHelper.IsKeyUp(settings.AssignedInput);

            if(hasClicked)
            {
                resetHasClickedSeconds -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(resetHasClickedSeconds < 0.0f)
                {
                    hasClicked = false;
                }
            }

            // put small collision into textgroup later after we do some calculations
            if (Enabled && checkInput && (hasClicked || keyPressed))
            {
                hasClicked = false;

                if(!settings.CanClickMultipleTimes)
                {
                    checkInput = false;
                }

                Text.Letters.ForEach(x =>
                {
                    x.Run(ClickAnimation(x.Id, x.Position, Text.IsLast(x.Id)));
                });
            }

        }

        private IAction3D ClickAnimation(int id, Vector3 originalPosition, bool isLast = false)
        {
            var wait = ActionBuilder.Wait(0.3f * settings.ClickAnimationDuration * id);

            var spin1 = ActionBuilder.RotateY(180f, settings.ClickAnimationDuration);
            var scaleUp = ActionBuilder.MoveBy(settings.ClickAnimationMoveBy, settings.ClickAnimationDuration);
            var group1 = ActionBuilder.Group(spin1, scaleUp);


            var spin2 = ActionBuilder.RotateY(360f, settings.ClickAnimationDuration);
            var scaleDown = ActionBuilder.MoveTo(originalPosition, settings.ClickAnimationDuration);
            var group2 = ActionBuilder.Group(spin2, scaleDown);

            var raiseEvent = ActionBuilder.RaiseEvent<T>(settings.RaisedEventOnClick, null);

            if (isLast)
            {
                return ActionBuilder.Sequence(wait, group1, group2, raiseEvent);
            }
            else
            {
                return ActionBuilder.Sequence(wait, group1, group2);
            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Text.Draw(view, projection);
        }

        public void SetAlpha(float alpha)
        {
            Text.SetAlpha(alpha);
        }

    }
}
