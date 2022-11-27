using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProximityND.Config;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public static class ScreenManager<T>
    {
        static Dictionary<T, IScreen> Screens = new Dictionary<T, IScreen>();

        static IScreen CurrentScreen = null;

        static MouseState LastMouseState;

        public static void SetNewResolution(int newWidth, int newHeight)
        {
            if(ScreenSettings.Graphics != null)
            {
                ScreenSettings.Graphics.PreferredBackBufferWidth = newWidth;
                ScreenSettings.Graphics.PreferredBackBufferHeight = newHeight;
                ScreenSettings.Graphics.ApplyChanges();
                if(CurrentScreen != null)
                {
                    CurrentScreen.Resize(newWidth, newHeight);
                }
            }
        }

        public static void Load(T screenType, IScreen screen)
        {
            Screens[screenType] = screen;
        }
        public static void SwitchTo(T screen)
        {
            if(CurrentScreen != null)
            {
                CurrentScreen.Cleanup();
            }

            CurrentScreen = Screens[screen];

            CurrentScreen.Initialize();
        }

        public static void Update(ScreenUpdateCommand command)
        {
            InputHelper.UpdateBefore();

            CurrentScreen.Update(command.GameTime);

            var currentMouseState = Mouse.GetState();

            var hasMoved = currentMouseState.Y != LastMouseState.Y || currentMouseState.X != LastMouseState.X;

            // TODO: switch how mouse is handled to events fired to pubhub, make a backbone class, handle 
            var mouseState = (currentMouseState.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed) ? MouseEvent.Release :
                                (currentMouseState.LeftButton == ButtonState.Pressed) ? MouseEvent.Pressed :
                                hasMoved ? MouseEvent.Moved :
                                MouseEvent.None;


            if (mouseState != MouseEvent.None)
            {
                Vector2 mouseLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                var mouseToWorldPosX = mouseLocation.X - command.Viewport.Width / 2f;
                var mouseToWorldPosY = (mouseLocation.Y - command.Viewport.Height / 2f) * -1f;
                var worldPosition = new Vector2(mouseToWorldPosX, mouseToWorldPosY);

                var handleMouseCommand = new HandleMouseCommand()
                {
                    MousePosition = mouseLocation,
                    Projection = command.Projection,
                    View = command.View,
                    Viewport = command.Viewport,
                    State = mouseState,
                    WorldPosition = worldPosition
                };

                CurrentScreen.HandleMouse(handleMouseCommand);
            }

            LastMouseState = currentMouseState;

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
