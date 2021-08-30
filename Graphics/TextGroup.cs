using Backbone.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public class TextGroup : IGUI3D
    {
        static Dictionary<char, float> LetterWidths = new Dictionary<char, float>() {
            { '0', 0.507f },
            { '1', 0.507f },
            { '2', 0.507f },
            { '3', 0.507f },
            { '4', 0.507f },
            { '5', 0.507f },
            { '6', 0.507f },
            { '7', 0.507f },
            { '8', 0.507f },
            { '9', 0.507f },
            { 'A', 0.507f },
            { 'B', 0.49f },
            { 'C', 0.5f },
            { 'D', 0.48f },
            { 'E', 0.38f },
            { 'F', 0.43f },
            { 'G', 0.5f },
            { 'H', 0.48f },
            { 'I', 0.25f },
            { 'J', 0.35f },
            { 'K', 0.5f },
            { 'L', 0.4f },
            { 'M', 0.65f },
            { 'N', 0.48f },
            { 'O', 0.5f },
            { 'P', 0.49f },
            { 'Q', 0.49f },
            { 'R', 0.49f },
            { 'S', 0.49f },
            { 'T', 0.49f },
            { 'U', 0.49f },
            { 'V', 0.56f },
            { 'W', 0.80f },
            { 'X', 0.49f },
            { 'Y', 0.52f },
            { 'Z', 0.43f },
            { ' ', 0.38f },
            { ':', 0.25f },
            { '<', 0.52f },
            { '>', 0.52f },
            { 'É', 0.38f },
            { '!', 0.25f },
            { '\\', 0.35f },
            { '/', 0.35f }
        };

        List<Movable3D> letters = new List<Movable3D>();
        float baseScale = 0f;
        Movable3D parent = null;
        ColorType textColor = ColorType.None;

        public int Value { get; private set; } = 0;

        public int Id { get; private set; } = 0;

        public float Left { get; set; }
        public float Right { get; set; }
        public Vector3 Position { get; set; } = Vector3.Zero;

        public Action3D TransitionInAnimation { get; set; } = null;
        public Action3D TransitionOutAnimation { get; set; } = null;


        public TextGroup(int id, Movable3D parent, Vector3 position, float scale)
        {
            this.Id = id;
            this.baseScale = scale;
            this.Position = position;
            this.parent = parent;
        }

        public void SetColor(ColorType color)
        {
            //.ForEach(x => x.MeshColors[ModelStore.MeshNames[MeshType.TextCharacter]] = color);
        }

        public void SetText(string text)
        {
            // TODO: remove once we have lower case letters
            text = text.ToUpper();

            letters.Clear();

            var length = GetTotalLength(text);
            var currentPositionX = Position.X - (length / 2f);

            Left = currentPositionX;

            for (int i = 0; i < text.Length; i++)
            {
                var character = text[i];

                if(character != ' ')
                {
                    var halfCharWidth = 0.5f * (baseScale * LetterWidths[text[i]]);
                    var currentPlusHalfChar = currentPositionX + halfCharWidth;

                    /*
                    var model = new Movable3D(ModelStore.Letters[character], new Vector3(currentPlusHalfChar, Position.Y, Position.Z), baseScale);
                    model.Parent = parent;
                    model.RotationY = 0f;
                    model.RotationX = 0f;

                    if(textColor != ColorType.None)
                    {
                        model.MeshColors[ModelStore.MeshNames[MeshType.TextCharacter]] = textColor;
                    }

                    letters.Add(model);*/

                    if(i == (text.Length - 1))
                    {
                        // add the other half of the char width to get the right side
                        Right = currentPlusHalfChar + halfCharWidth;
                    }
                }

                currentPositionX += baseScale * LetterWidths[text[i]];
            }
        }

        public float GetTotalLength(string digits)
        {
            var length = 0f;
            foreach(var digit in digits)
            {
                length += baseScale * LetterWidths[digit];
            }
            return length;
        }

        public void Run(Action3D action, bool replaceExisting = false)
        {
            letters.ForEach(x => x.Run(action, replaceExisting));
        }

        public void Draw(Matrix view, Matrix projection)
        {
            letters.ForEach(x => x.Draw(view, projection));
        }

        public void HandleMouse(Vector2 mousePosition, Matrix view, Matrix projection, Viewport viewport)
        {
        }

        public IAction3D IdleAnimation2(int characterIndex, Vector3 origPosition)
        {
            //var star
            //var moveTo = ActionBuilder.MoveTo()
            var wait = ActionBuilder.Wait(characterIndex * 0.3f + 2.0f);
            var rotateIn = ActionBuilder.RotateX(20f, 0.3f);
            var rotateBack = ActionBuilder.RotateX(0f, 0.3f);
            var wait2 = ActionBuilder.Wait(8.0f);
            var sequence = ActionBuilder.Sequence(wait, rotateIn, rotateBack, wait2);
            return sequence;
        }

        public IAction3D IdleAnimation(int characterIndex)
        {
            var wait = ActionBuilder.Wait(characterIndex * 0.3f + 2.0f);
            var rotateIn = ActionBuilder.RotateX(20f, 0.3f);
            var rotateBack = ActionBuilder.RotateX(0f, 0.3f);
            var wait2 = ActionBuilder.Wait(8.0f);
            var sequence = ActionBuilder.Sequence(wait, rotateIn, rotateBack, wait2);
            return sequence;
        }

        public void Update(GameTime gameTime)
        {
            /*
            letters.ForEach(x =>
            {
                x.RotationX -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100f;
                x.RotationY -= (float)gameTime.ElapsedGameTime.TotalSeconds * 235f;
                x.RotationZ -= (float)gameTime.ElapsedGameTime.TotalSeconds * 143f;
            });*/

            letters.ForEach(x => x.Update(gameTime));
        }

        public void UpdateRelativeToParent()
        {
            throw new NotImplementedException();
        }

        public void TransitionOut()
        {
            if(this.TransitionOutAnimation != null)
            {
                this.Run(this.TransitionOutAnimation);
            }
        }

        public void TransitionIn()
        {
            if(this.TransitionInAnimation != null)
            {
                this.Run(TransitionInAnimation);
            }
        }
    }
}
