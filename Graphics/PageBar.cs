using Backbone.Events;
using Backbone.Graphics;
using Backbone.Input;
using Backbone.UI;
using Microsoft.Xna.Framework;
using ProximityND.Enums;
using System;

namespace ProximityND.Backbone.Graphics
{
    public class PageBarSettings<T>
    {
        public T LeftArrowEvent { get; set; }
        public T RightArrowEvent { get; set; }
        public Movable3D ParentMovable { get; set; } = Movable3D.Empty();
        public Vector3 Position { get; set; } = Vector3.Zero;
        public float Scale { get; set; } = 1f;
        public Func<string> GetHeaderColor { get; set; } = null;
        public Func<string> GetHeaderText { get; set; } = null;

    }

    public class PageBar<T> : IGUI3D
    {
        private TextButton<T> LeftArrowButton = null;
        private TextButton<T> RightArrowButton = null;

        private TextGroup tabNameText = null;
        private Func<string> getHeaderColor { get; set; } = null;
        private Func<string> getHeaderText { get; set; } = null;

        public PageBar(PageBarSettings<T> settings)
        {
            var initialHeaderText = (getHeaderText != null) ? getHeaderText() : "HEADER";
            var initialColor = (getHeaderColor != null) ? getHeaderColor() : "#FFFFFF";

            // Initialize tabNameText
            tabNameText = new TextGroup(new TextGroupSettings()
            {
                Color = initialColor,
                Parent = settings.ParentMovable,
                Position = settings.Position,
                Scale = settings.Scale,
                Text = initialHeaderText,
            });

            LeftArrowButton = CreateArrowButton("<", settings.LeftArrowEvent, InputAction.LeftShoulder, new Vector3(settings.Position.X - 300f, settings.Position.Y, settings.Position.Z));
            RightArrowButton = CreateArrowButton(">", settings.RightArrowEvent, InputAction.RightShoulder, new Vector3(settings.Position.X + 300f, settings.Position.Y, settings.Position.Z));

            getHeaderColor = settings.GetHeaderColor;
            getHeaderText = settings.GetHeaderText;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            tabNameText.Draw(view, projection);
            LeftArrowButton.Draw(view, projection);
            RightArrowButton.Draw(view, projection);
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            LeftArrowButton.HandleMouse(command);
            RightArrowButton.HandleMouse(command);
        }

        public void TransitionIn()
        {
        }

        public void TransitionOut()
        {
        }

        public void Update(GameTime gameTime)
        {
            if(getHeaderText != null)
            {
                var newHeaderText = getHeaderText();
                if (newHeaderText != null && !newHeaderText.Equals(tabNameText.Text))
                {
                    tabNameText.SetText(newHeaderText);
                }
            }

            if (getHeaderColor != null)
            {
                var newHeaderColor = getHeaderColor();
                if(!string.IsNullOrWhiteSpace(newHeaderColor) && !newHeaderColor.Equals(tabNameText.Color))
                {
                    tabNameText.SetColor(newHeaderColor);
                }
            }

            tabNameText.Update(gameTime);
            LeftArrowButton.Update(gameTime);
            RightArrowButton.Update(gameTime);
        }

        private TextButton<T> CreateArrowButton(string text, T clickEvent, InputAction assignedInput, Vector3 position)
        {
            return new TextButton<T>(new TextButtonSettings<T>()
            {
                AssignedInput = assignedInput,
                RaisedEventOnClick = clickEvent,
                LetterCollisionRadius = 0.65f,
                ClickAnimationMoveBy = new Vector3(0f, -10f, 0f),
                ClickAnimationDuration = 0.15f,
                CanClickMultipleTimes = true,
                TextGroupSettings = new TextGroupSettings()
                {
                    Color = ProviderHub<string, ThemeElementType>.Request(ThemeElementType.ButtonTextColor),
                    Id = 0,
                    Parent = Movable3D.Empty(),
                    Position = position,
                    Scale = 100f,
                    Text = text,
                }
            });
        }
    }
}
