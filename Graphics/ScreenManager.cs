using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public static class ScreenManager<T>
    {
        static Dictionary<T, IScreen> Screens = new Dictionary<T, IScreen>();

        static IScreen CurrentScreen = null;



        public static void Load(T screenType, IScreen screen)
        {
            Screens[screenType] = screen;
        }
        public static void SwitchTo(T screen)
        {
            CurrentScreen = Screens[screen];
            CurrentScreen.Initialize();
        }

        public static void Update(ScreenUpdateCommand command)
        {
            InputHelper.UpdateBefore();

            CurrentScreen.Update(command.GameTime);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 mouseLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                var handleMouseCommand = new HandleMouseCommand()
                {
                    MousePosition = mouseLocation,
                    Projection = command.Projection,
                    View = command.View,
                    Viewport = command.Viewport
                };

                CurrentScreen.HandleMouse(handleMouseCommand);
            }

            InputHelper.UpdateAfter();
        }

        public static void Draw(Matrix view, Matrix projection)
        {
            CurrentScreen.Draw(view, projection);
        }

        public static void DrawText(SpriteBatch spriteBatch)
        {
            CurrentScreen.DrawText(spriteBatch);
        }
    }
}
