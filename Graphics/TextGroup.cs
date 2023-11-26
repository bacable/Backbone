using Backbone.Actions;
using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Backbone.Events;
using ProximityND.Backbone.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Backbone.UI;
using System.Windows.Forms;
using System.Diagnostics;

namespace Backbone.Graphics
{
    public class TextGroup : IGUI3D
    {
        // NOTE: Set this in your game before you use a TextGroup! Just populate the Dictionary with the acceptable characters (0-1,A-Z, accented characters and punctuation, etc), plus how wide they are compared to the other letter
        // Example: { '0', 0.507f }, { 'L', '0.4f' }.... L in this character set has a smaller width than the 0, so it will be a smaller number here.
        // Very trial and error here right now, not automatically calculated.
        public static Dictionary<char, float> LetterWidths { get; set; } = new Dictionary<char, float>();

        // NOTE: set this to the name of the mesh in the model that you want to control the color for
        public static string MeshNameToColor = string.Empty;

        // NOTE: set this so each supported character has a Monogame Model object associated with it
        public static Dictionary<char, Model> LetterModels { get; set; } = new Dictionary<char, Model>();

        public List<Movable3D> Letters { get; private set; } = new List<Movable3D>();

        private TextAlign alignment = TextAlign.Center;

        public string Text { get; private set; } = string.Empty;

        public Matrix? World { get
            {
                return parent.World;
            } }

        private BoundingSphere groupBoundingSphere = default(BoundingSphere);
        private bool recalculateBoundingSphere = true;
        public BoundingSphere GroupBoundingSphere
        {
            get
            {
                if(recalculateBoundingSphere)
                {
                    var distance = Math.Sqrt(Math.Pow((Left + Right), 2));
                    var radius = (float)(distance / 2f);
                    var center = new Vector3((Left + Right) / 2f, Position.Y, Position.Z);
                    groupBoundingSphere = new BoundingSphere(center, radius);
                    recalculateBoundingSphere = false;
                }
                return groupBoundingSphere;
            }
        }

        private Rectangle boundingBox = default(Rectangle);
        private bool recalculateBoundingBox = true;
        public Rectangle BoundingBox
        {
            get
            {
                if(recalculateBoundingBox)
                {
                    var center = new Vector3((Left + Right) / 2f, Position.Y, Position.Z);
                    var width = Right - Left;
                    boundingBox = new Rectangle((int)center.X, (int)center.Y, (int)width, 100); //TODO: fix the height
                    recalculateBoundingBox = false;
                }
                return boundingBox;
            }
        }

        float baseScale = 0f;
        Movable3D parent = null;
        string textColor = string.Empty;

        public int Value { get; private set; } = 0;

        public int Id { get; private set; } = 0;

        public float Left { get; set; }
        public float Right { get; set; }

        public string Color { get; set; }
        public Vector3 Position { get; set; } = Vector3.Zero;

        // first is the int to pass in the index of the letter (matters for some anims),
        // second param is the resulting animation
        public Func<int, IAction3D> TransitionOutAnimation = null;
        public Func<int, IAction3D> TransitionInAnimation = null;

        public bool IsAnimating
        {
            get
            {
                return Letters.Any(letter => letter.IsAnimating);
            }
        }

        public TextGroup(TextGroupSettings settings)
        {
            this.Id = settings.Id;
            this.baseScale = settings.Scale;
            this.Position = settings.Position;
            this.parent = settings.Parent;
            this.alignment = settings.Alignment;

            if(settings.TransitionInAnim!= null)
            {
                TransitionInAnimation = settings.TransitionInAnim;
            }

            if(settings.TransitionOutAnim!= null)
            {
                TransitionOutAnimation = settings.TransitionOutAnim;
            }

            if(settings.Color != string.Empty)
            {
                SetColor(settings.Color);
            }
            else
            {
                var textColor = ProviderHub<string, UIElementColorType>.Request(UIElementColorType.TextGroupDefaultTextColor);
                SetColor(textColor);
            }

            if(!string.IsNullOrEmpty(settings.Text))
            {
                SetText(settings.Text);
            }

            if (LetterWidths.Count == 0 || LetterModels.Count == 0 || string.IsNullOrEmpty(MeshNameToColor))
            {
                throw new Exception("Please set these before using this class or else things will break.");
            }
        }

        public void SetColor(string color)
        {
            Letters.ForEach(x => x.MeshProperties[MeshNameToColor] = new MeshProperty()
            {
                Color = ColorHex.Get(color)
            });
            textColor = color;
        }

        public bool IsLast(int letterId)
        {
            return letterId == (Letters.Count - 1);
        }


        public void SetText(string text)
        {
            List<char> charsWithoutModels = new List<char>();
            List<char> charsWithoutWidths = new List<char>();

            // TODO: remove once we have lower case letters
            text = text.ToUpper();

            if (Text.Equals(text)) return; // don't do any updates if the text hasn't changed, especially since this can destroy existing animations

            Text = text;

            Letters.Clear();

            var length = GetTotalLength(text);
            var currentPositionX = (alignment == TextAlign.Center) ?
                Position.X - (length / 2f) :
                (alignment == TextAlign.Left) ?
                Position.X :
                Position.X - length;

            Left = currentPositionX;

            for (int i = 0; i < text.Length; i++)
            {
                var character = 
                    text[i];

                if(character != ' ')
                {
                    if (!LetterModels.ContainsKey(text[i]))
                    {
                        charsWithoutModels.Add(text[i]);
                        character = '-';
                    }

                    if (!LetterWidths.ContainsKey(text[i]))
                    {
                        charsWithoutWidths.Add(text[i]);
                        character = '-';
                    }

                    var halfCharWidth = 0.5f * (baseScale * LetterWidths[character]);
                    var currentPlusHalfChar = currentPositionX + halfCharWidth;

                    var model = new Movable3D(LetterModels[character], new Vector3(currentPlusHalfChar, Position.Y, Position.Z), baseScale);
                    model.Parent = parent;
                    model.RotationY = 0f;
                    model.RotationX = 0f;
                    model.Id = i;

                    if(textColor != string.Empty)
                    {
                        model.MeshProperties[MeshNameToColor] = new MeshProperty()
                        {
                            Color = ColorHex.Get(textColor)
                        };
                    }

                    Letters.Add(model);

                    if(i == (text.Length - 1))
                    {
                        // add the other half of the char width to get the right side
                        Right = currentPlusHalfChar + halfCharWidth;
                    }
                }

                currentPositionX += baseScale * LetterWidths[character];
            }

            if(charsWithoutWidths.Count > 0)
            {
                Debug.WriteLine("Missing character widths: " + string.Join(',', charsWithoutWidths));
            }

            if (charsWithoutModels.Count > 0)
            {
                Debug.WriteLine("Missing character models: " + string.Join(',', charsWithoutModels));
            }
        }

        public float GetTotalLength(string digits)
        {
            var length = 0f;
            foreach(var digit in digits)
            {
                var digitWidth = LetterWidths.ContainsKey(digit) ? LetterWidths[digit] : LetterWidths[' '];
                length += baseScale * digitWidth;
            }
            return length;
        }

        public void Run(Func<int, IAction3D> animationFunction, bool replaceExisting = false)
        {
            for(int index = 0; index < Letters.Count; index++)
            {
                var letter = Letters[index];
                var anim = animationFunction(index);
                letter.Run(anim, replaceExisting);
            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Letters.ForEach(x => x.Draw(view, projection));
        }

        public void HandleMouse(HandleMouseCommand command)
        {
        }

        public IAction3D IdleAnimation2(int characterIndex, Vector3 origPosition)
        {
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
            // If the base is moving, we will need to recalculate the bounding sphere for
            // collision
            if(parent != null && parent.IsAnimating)
            {
                recalculateBoundingSphere = true;
            }

            Letters.ForEach(x => x.Update(gameTime));
        }

        public void UpdateRelativeToParent()
        {
            throw new NotImplementedException();
        }

        public void TransitionOut()
        {
            if(this.TransitionOutAnimation != null)
            {
                this.Run(this.TransitionOutAnimation, true);
            }
        }

        public void TransitionIn()
        {
            if(this.TransitionInAnimation != null)
            {
                this.Run(TransitionInAnimation, true);
            }
        }

        public void AddPositionToText(Vector3 add)
        {
            for(var i=0; i<Letters.Count; i++)
            {
                var letter = Letters[i];
                letter.UpdatePosition(new Vector3(letter.Position.X + add.X, letter.Position.Y + add.Y, letter.Position.Z + add.Z));
            }
        }
    }
}
