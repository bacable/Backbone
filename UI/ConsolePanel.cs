using Backbone.Events;
using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Backbone.UI
{
    public class ConsolePanel : IGUI3D, ISubscriber<ConsolePanelEvent>
    {
        public List<ConsoleItem> Items = new List<ConsoleItem>();

        public void Draw(Matrix view, Matrix projection)
        {
        }
        public void HandleEvent(ConsolePanelEvent eventType, object payload)
        {
            switch(eventType)
            {
                case ConsolePanelEvent.AddLine:

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
