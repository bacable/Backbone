using Backbone.Actions;
using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backbone.UI
{
    public class TextButton<T> : IGUI3D
    {
        private TextGroup Text;

        TextButtonSettings<T> settings;

        bool hasClicked = false;
        bool checkInput = true;

        public TextButton(TextButtonSettings<T> settings)
        {
            this.settings = settings;
            
            Text = new TextGroup(settings.TextGroupSettings);
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            if(Collision3D.HasSphereCollision(command, Text.Letters, settings.LetterCollisionRadius, Text.World.Value, Text.GroupBoundingSphere))
            {
                hasClicked = true;
            }
        }

        public void TransitionIn()
        {
        }

        public void TransitionOut()
        {
        }

        public void Update(GameTime gameTime)
        {
            Text.Update(gameTime);

            var keyPressed = InputHelper.IsKeyUp(settings.AssignedInput);

            // put small collision into textgroup later after we do some calculations
            if (checkInput && (hasClicked || keyPressed))
            {
                string collisionfound = "BUTTON COLLISION";
                VariableMonitor.Report(m => collisionfound, 0.1f);
                hasClicked = false;
                checkInput = false;

                Text.Letters.ForEach(x =>
                {
                    x.Run(ClickAnimation(x.Position));
                });
            }

        }

        private IAction3D ClickAnimation(Vector3 originalPosition)
        {
            var scaleUp = ActionBuilder.MoveBy(settings.ClickAnimationMoveBy, settings.ClickAnimationDuration);
            var scaleDown = ActionBuilder.MoveTo(originalPosition, settings.ClickAnimationDuration);
            var raiseEvent = ActionBuilder.RaiseEvent<T>(settings.RaisedEventOnClick, null);
            var sequence = ActionBuilder.Sequence(scaleUp, scaleDown);
            return sequence;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Text.Draw(view, projection);
        }

    }
}
