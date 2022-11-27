using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProximityND.Config;
using System.Collections.Generic;
using System.Diagnostics;

namespace Backbone.Graphics
{
    public static class ScreenManager<T>
    {
        static Dictionary<T, IScreen> Screens = new Dictionary<T, IScreen>();

        static IScreen CurrentScreen = null;

        static MouseState LastMouseState;

        public static void SetNewResolution(string newResolution)
        {
            if(ScreenSettings.Graphics != null && !string.IsNullOrEmpty(newResolution))
            {
                var resolutionSplit = newResolution.Split('x');
                int width, height;
                if (resolutionSplit.Length > 1)
                {
                    int.TryParse(resolutionSplit[0], out width);
                    int.TryParse(resolutionSplit[1], out height);

                    ScreenSettings.Graphics.PreferredBackBufferWidth = width;
                    ScreenSettings.Graphics.PreferredBackBufferHeight = height;
                    ScreenSettings.Graphics.ApplyChanges();
                    if (CurrentScreen != null)
                    {
                        CurrentScreen.Resize(width, height);
                    }
                }
            }
        }

        public static void SetFullScreen(bool isFullScreen)
        {
            ScreenSettings.Graphics.IsFullScreen = isFullScreen;
            ScreenSettings.Graphics.ApplyChanges();
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

                var mouseToWorldPosX = mouseLocation.X - ScreenSettings.Width / 2;
//                    command.Viewport.Width / 2f;
                var mouseToWorldPosY = (mouseLocation.Y - ScreenSettings.Height / 2f) * -1f;
                var worldPosition = new Vector2(mouseToWorldPosX, mouseToWorldPosY);

                //Debug.WriteLine("Mouse X:" + mouseLocation.X, ", Y: " + mouseLocation.Y);
                //Debug.WriteLine("World X:" + worldPosition.X, ", Y: " + worldPosition.Y);

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
