﻿using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Backbone.Graphics
{
    public static class ScreenManager<T>
    {
        static IScreen CurrentScreen = null;

        static MouseState LastMouseState;

        static int PreviousMouseScroll;

        static Dictionary<T, IScreen> Screens = new Dictionary<T, IScreen>();

        static Stack<IScreen> ScreenStack = new Stack<IScreen>();

        private static List<OverlayInfo> OverlayInfos = new List<OverlayInfo>();

        public static SpriteBatch OverlaySpriteBatch { get; set; }

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
                    
                    ScreenSettings.ResolutionWidth = width;
                    ScreenSettings.ResolutionHeight = height;

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
        public static void SwitchTo(T screen, int delayMilliseconds = 0)
        {
            if(delayMilliseconds > 0)
            {
                Task.Delay(delayMilliseconds).ContinueWith(t =>
                {
                    SwitchToScreen(screen);
                });
            } else
            {
                SwitchToScreen(screen);
            }
        }

        private static void SwitchToScreen(T screen)
        {
            if (ScreenStack.Count > 0)
            {
                while (ScreenStack.Count > 0)
                {
                    IScreen topScreen = ScreenStack.Pop();
                    topScreen.Cleanup();
                }
            }

            IScreen newScreen = Screens[screen];
            newScreen.Initialize();
            ScreenStack.Push(newScreen);

        }

        public static void PushOverlayScreen(T overlay)
        {
            IScreen newOverlay = Screens[overlay];
            newOverlay.Initialize();
            ScreenStack.Push(newOverlay);
            OverlayInfos.Add(new OverlayInfo { CurrentAlpha = 0, TargetAlpha = 204, IsFading = true });
        }

        public static void PopOverlay()
        {
            if (ScreenStack.Count > 0)
            {
                int lastIndex = OverlayInfos.Count - 1;
                if (lastIndex >= 0)
                {
                    OverlayInfos[lastIndex].TargetAlpha = 0;
                    OverlayInfos[lastIndex].IsFading = true;
                }
            }
        }

        private static void DrawFullScreenOverlay(int index)
        {
            Texture2D pixel = new Texture2D(OverlaySpriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.Black });

            Color overlayColor = new Color(0, 0, 0, (int)OverlayInfos[index].CurrentAlpha);

            OverlaySpriteBatch.Begin();
            OverlaySpriteBatch.Draw(pixel, new Rectangle(0, 0, ScreenSettings.ResolutionWidth, ScreenSettings.ResolutionHeight), overlayColor);
            OverlaySpriteBatch.End();
        }

        public static void Update(ScreenUpdateCommand command)
        {
            try
            {
                if (ScreenStack.Count == 0)
                {
                    return;
                }

                InputHelper.UpdateBefore();

                IScreen topScreen = ScreenStack.Peek();
                topScreen.Update(command.GameTime);

                var currentMouseState = Mouse.GetState();

                var hasMoved = currentMouseState.Y != LastMouseState.Y || currentMouseState.X != LastMouseState.X;

                // TODO: switch how mouse is handled to events fired to pubhub, make a backbone class, handle 
                var mouseState = (currentMouseState.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed) ? MouseEvent.LeftButtonReleased :
                                    (currentMouseState.LeftButton == ButtonState.Pressed) ? MouseEvent.LeftButtonPressed :
                                    (currentMouseState.RightButton == ButtonState.Released && LastMouseState.RightButton == ButtonState.Pressed) ? MouseEvent.RightButtonReleased :
                                    (currentMouseState.RightButton == ButtonState.Pressed) ? MouseEvent.RightButtonPressed :
                                    MouseEvent.None;

                // Positive number = scroll down, negative number = scroll up.
                var mouseScroll = (PreviousMouseScroll - currentMouseState.ScrollWheelValue) / 12.0f;

                PreviousMouseScroll = currentMouseState.ScrollWheelValue;

                if (mouseState != MouseEvent.None || hasMoved)
                {
                    RumbleManager.IsActive = false;

                    Vector2 mouseLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                    var mouseToWorldPosX = mouseLocation.X - ScreenSettings.ResolutionWidth / 2;
                    var mouseToWorldPosY = (mouseLocation.Y - ScreenSettings.ResolutionHeight / 2f) * -1f;
                    var worldPosition = new Vector2(mouseToWorldPosX, mouseToWorldPosY);

                    var handleMouseCommand = new HandleMouseCommand()
                    {
                        MousePosition = mouseLocation,
                        MouseScroll = mouseScroll,
                        Projection = command.Projection,
                        View = command.View,
                        Viewport = command.Viewport,
                        State = mouseState,
                        HasMoved = hasMoved,
                        WorldPosition = worldPosition,
                        Ratio = new Vector2(ScreenSettings.HorizontalRatio, ScreenSettings.VerticalRatio),
                    };

                    topScreen.HandleMouse(handleMouseCommand);
                }

                LastMouseState = currentMouseState;

                for (int i = 0; i < OverlayInfos.Count; i++)
                {
                    OverlayInfo info = OverlayInfos[i];
                    if (info.IsFading)
                    {
                        float alphaDifference = info.TargetAlpha - info.CurrentAlpha;
                        float alphaChange = alphaDifference * (float)command.GameTime.ElapsedGameTime.TotalSeconds * 6.0f;
                        info.CurrentAlpha += alphaChange;

                        if (Math.Abs(info.CurrentAlpha - info.TargetAlpha) < 1)
                        {
                            info.CurrentAlpha = info.TargetAlpha;
                            info.IsFading = false;

                            if (info.TargetAlpha == 0)
                            {
                                IScreen topScreen2 = ScreenStack.Pop();
                                topScreen2.Cleanup();
                                OverlayInfos.RemoveAt(i);
                            }
                        }
                    }
                }

                InputHelper.UpdateAfter();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ScreenManager:Update - {0}", ex.Message);
            }
        }

        public static void Draw(Matrix view, Matrix projection, SpriteBatch spriteBatch)
        {
            if (ScreenStack.Count == 0)
            {
                return;
            }

            var screens = ScreenStack.Reverse().ToArray();
            float zOffset = 0f;
            float zOffsetStep = 30f; //TODO: make this configurable later

            for (int i = 0; i < screens.Length; i++)
            {
                foreach (DrawLayerType layer in Enum.GetValues(typeof(DrawLayerType)))
                {
                    if(layer == DrawLayerType.DrawText)
                    {
                        spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    }

                    ScreenSettings.Graphics.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
                    ScreenSettings.Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    Matrix offsetView = view * Matrix.CreateTranslation(0, 0, zOffset);
                    screens[i].Draw(layer, offsetView, projection, spriteBatch);

                    if(layer == DrawLayerType.DrawText)
                    {
                        spriteBatch.End();
                    }

                }

                if (i < OverlayInfos.Count)
                {
                    DrawFullScreenOverlay(i);
                }

                zOffset += zOffsetStep;
            }
        }
    }
}
