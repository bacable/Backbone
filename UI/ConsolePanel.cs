using Backbone.Events;
using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Backbone.UI
{
    public class ConsolePanel : IGUI3D, ISubscriber<ConsolePanelEvent>
    {
        public List<ConsoleItem> Items = new List<ConsoleItem>();

        public ConsolePanel()
        {
            AddExampleLog();
        }

        public void AddExampleLog()
        {
            Items.Add(new ConsoleItem()
            {
                Message = "votes to play at A4",
                User = "RandomUser1",
                StartDate = DateTime.Now,
                UserColor = Color.Blue
            });
            Items.Add(new ConsoleItem()
            {
                Message = "votes to play at G7",
                User = "RandomUser88",
                StartDate = DateTime.Now,
                UserColor = Color.Purple
            });
            Items.Add(new ConsoleItem()
            {
                Message = "votes to play at N5",
                User = "RandomUser3",
                StartDate = DateTime.Now,
                UserColor = Color.Navy
            });
        }

        public void Draw(Matrix view, Matrix projection)
        {
        }

        public void DrawText(SpriteBatch spriteBatch)
        {

        }

        public void HandleEvent(ConsolePanelEvent eventType, object payload)
        {
            switch(eventType)
            {
                case ConsolePanelEvent.AddLine:
                    Items.Add((ConsoleItem)payload); break;
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
        }
    }
}
