using Backbone.Graphics;
using Backbone.Input;
using Backbone.Menus;
using Microsoft.Xna.Framework;

namespace Backbone.UI
{
    public class InputTextbox : IGUI3D
    {
        #region properties
        public bool IsActive { get; set; } = false;
        public string Value { get { return input.Value; } }
        #endregion

        #region private fields
        private TextGroup text { get; set; }
        private InputBox input { get; set; }
        #endregion

        public InputTextbox(InputTextboxSettings settings)
        {
            text = new TextGroup(settings.TextGroupSettings);
            input = settings.InputBox;
            IsActive = input.IsEditing;
        }

        public void HandleMouse(HandleMouseCommand command)
        {
        }

        public void TransitionIn()
        {
        }

        public void TransitionOut()
        {
        }

        public void Update(GameTime gameTime)
        {
            if(IsActive)
            {
                var oldValue = input.Value;
                input.Value = InputHelper.UpdateTypedString(input.Value);

                if(!oldValue.Equals(input.Value))
                {
                    text.SetText(input.Value);
                }
            }

            text.Update(gameTime);
        }
        public void Draw(Matrix view, Matrix projection)
        {
            text.Draw(view, projection);
        }

    }
}
