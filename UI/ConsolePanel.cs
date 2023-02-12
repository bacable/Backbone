using Backbone.Events;
using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProximityND;
using ProximityND.Config;
using ProximityND.Managers;
using ProximityND.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace Backbone.UI
{
    public class ConsolePanel : IGUI3D, ISubscriber<ConsolePanelEvent>
    {
        public List<ConsoleItem> Items = new List<ConsoleItem>();

        ConsolePanelSettings settings;

        Vector2 position;

        Color[] RandomColors = new Color[6] { Color.Red, Color.Pink, Color.Purple, Color.Green, Color.Blue, Color.Yellow };

        public ConsolePanel(ConsolePanelSettings settings)
        {
            this.settings = settings;
            position = settings.StartPosition;

            PubHub<ConsolePanelEvent>.Sub(new ConsolePanelEvent[]{
                ConsolePanelEvent.AddLine,
                ConsolePanelEvent.ClearAll
            }, this);
        }

        public ConsoleItem GetRandomLine()
        {
            var color = RandomColors[RandomNumberGenerator.GetInt32(RandomColors.Length)];
            return new ConsoleItem()
            {
                Message = "votes",
                User = "RandomUser" + RandomNumberGenerator.GetInt32(100),
                StartDate = DateTime.Now,
                UserColor = color
            };
        }

        public void Draw(Matrix view, Matrix projection)
        {
        }

        public void HandleEvent(ConsolePanelEvent eventType, object payload)
        {
            switch(eventType)
            {
                case ConsolePanelEvent.AddLine:
                    Items.Add((ConsoleItem)payload); break;
                case ConsolePanelEvent.ClearAll:
                    Items.Clear(); break;
                default: break;
            }
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
            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var item in Items)
            {
                item.FadeInAmount = Math.Clamp(item.FadeInAmount + (elapsed * settings.FadeSpeed), 0f, 1f);

                if((DateTime.Now - item.StartDate).TotalSeconds > settings.SecondsBeforeLineExpiration)
                {
                    item.FadeOutAmount = Math.Clamp(item.FadeOutAmount + (elapsed * settings.FadeSpeed), 0f, 1f);
                }
            }

            Items.RemoveAll(item => item.FadeOutAmount >= 1f);
        }


        public void DrawText(SpriteBatch spriteBatch)
        {
            var textPosition = new Vector2(position.X, position.Y);

            // get the most recent X number of lines depending on max mines, or start from the beginning if less than that
            var startingIndex = Math.Max(Items.Count - settings.MaxLines, 0);
            var lineNumber = 0;

            for (var i = startingIndex; i < Items.Count; i++)
            {
                var item = Items[i];

                var linePosition = new Vector2(textPosition.X + (1f - item.FadeInAmount) * settings.TransitionDistance + item.FadeOutAmount * settings.TransitionDistance, textPosition.Y + lineNumber * settings.LineHeight);

                // should accomodate for fading in at start and fading out at the end
                var opacity = Math.Min(1f - item.FadeOutAmount, item.FadeInAmount);

                var userTextWidth = settings.Font.MeasureString(item.User + " ").X * settings.TextScale;

                // draw user text in its color, then the message after
                spriteBatch.DrawString(
                    settings.Font,
                    item.User,
                    linePosition,
                    new Color(item.UserColor.R, item.UserColor.G, item.UserColor.B, opacity),
                    0f,
                    Vector2.Zero,
                    settings.TextScale,
                    SpriteEffects.None,
                    0);

                // draw the rest of the message in the regular text color
                spriteBatch.DrawString(
                    settings.Font,
                    item.Message,
                    new Vector2(linePosition.X + userTextWidth, linePosition.Y),
                    new Color(settings.TextColor.R, settings.TextColor.G, settings.TextColor.B, opacity),
                    0f,
                    Vector2.Zero,
                    settings.TextScale,
                    SpriteEffects.None,
                    0);


                lineNumber += 1;
            }

            
        }

    }
}
