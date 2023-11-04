﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Backbone.Input
{
    public class InputHelper
    {
        public static KeyboardState LastKeyboardState;
        public static KeyboardState CurrentKeyboardState;

        public static GamePadState CurrentGamePadState;
        public static GamePadState LastGamePadState;

        public static GamePadCapabilities GamePadCapabilities;

        public static bool IsTextTyping = false;

        public static Dictionary<InputAction, Keys> KeyMapping = new Dictionary<InputAction, Keys>()
        {
            [InputAction.Accept] = Keys.Space,
            [InputAction.Back] = Keys.Back,
            [InputAction.Left] = Keys.Left,
            [InputAction.Right] = Keys.Right,
            [InputAction.Up] = Keys.Up,
            [InputAction.Down] = Keys.Down,
            [InputAction.Select1] = Keys.NumPad1,
            [InputAction.Select2] = Keys.NumPad2,
            [InputAction.Select3] = Keys.NumPad3,
            [InputAction.Select4] = Keys.NumPad4,
            [InputAction.Select5] = Keys.NumPad5,
            [InputAction.Start] = Keys.Enter,
            [InputAction.SpecialAction1] = Keys.Z,
            [InputAction.SpecialAction2] = Keys.X,
            [InputAction.SpecialAction3] = Keys.C,
            [InputAction.SpecialAction4] = Keys.V,
            [InputAction.LeftShoulder] = Keys.Q,
            [InputAction.RightShoulder] = Keys.E,
            [InputAction.N0] = Keys.D0,
            [InputAction.N1] = Keys.D1,
            [InputAction.N2] = Keys.D2,
            [InputAction.N3] = Keys.D3,
            [InputAction.N4] = Keys.D4,
            [InputAction.N5] = Keys.D5,
            [InputAction.N6] = Keys.D6,
            [InputAction.N7] = Keys.D7,
            [InputAction.N8] = Keys.D8,
            [InputAction.N9] = Keys.D9
        };

        public static void UpdateBefore()
        {
            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
            GamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);
        }

        public static void UpdateAfter()
        {
            LastKeyboardState = CurrentKeyboardState;
            LastGamePadState = CurrentGamePadState;
        }

        /// <summary>
        /// Gets the typed keys from the last update. Based off the code posted here:
        /// https://www.gamedev.net/forums/topic/457783-net-xna-getting-text-from-keyboard/4038836/?topic_id=457783
        /// </summary>
        /// <returns></returns>
        public static string UpdateTypedString(string currentString)
        {
            if (!IsTextTyping) return currentString;

            var pressedKeys = CurrentKeyboardState.GetPressedKeys();

            foreach(Keys key in pressedKeys)
            {
                bool isLastStateUnpressed = LastKeyboardState.IsKeyUp(key);
                if (isLastStateUnpressed)
                {
                    if(key == Keys.Back) // consume all of this key, because it's included in isSupported function to keep it from triggering elsewhere, kinda hacky, maybe switch later
                    {
                        if (currentString.Length > 0)
                        {
                            currentString = currentString.Substring(0, currentString.Length - 1);
                        }
                    }
                    else if(isSupportedTextTypingKey(key)) // TODO: add typing support for spaces
                    {
                        // have to strip the "D" from the beginning of the number keys, otherwise just toString
                        var appendText = (key >= Keys.D0 && key <= Keys.D9) ?
                                            key.ToString().Substring(1) : key.ToString();

                        currentString += appendText;
                    }
                }
            }

            return currentString;
        }

        public static bool IsKeyPressing(InputAction action)
        {
            // Check gamepad first, if doesn't match that, then try
            // keyboard input
            if (GamePadCapabilities.GamePadType == GamePadType.GamePad)
            {
                // Check for thumbstick direction matching the action requested first, and return true
                // if they are pressed, otherwise check the buttons
                if (isThumbstickPressing(action))
                {
                    Debug.WriteLine("ThumbstickPressed: " + isThumbstickPressing(action).ToString());
                    return true;
                }

                if (isButtonPressing(action))
                {
                    return true;
                }
            }

            if (KeyMapping.ContainsKey(action))
            {
                var key = KeyMapping[action];

                // don't want to trigger these actions if they're currently typing
                // in a text box
                if (IsTextTyping && isSupportedTextTypingKey(key)) return false;

                return CurrentKeyboardState.IsKeyDown(key);
            }

            return false;

        }

        public static  bool IsKeyPressed(InputAction action)
        {
            // Check gamepad first, if doesn't match that, then try
            // keyboard input
            if (GamePadCapabilities.GamePadType == GamePadType.GamePad)
            {
                // Check for thumbstick direction matching the action requested first, and return true
                // if they are pressed, otherwise check the buttons
                if (isThumbstickPressed(action))
                {
                    return true;
                }

                if (isButtonPressed(action))
                {
                    return true;
                }
            }

            if (KeyMapping.ContainsKey(action))
            {
                var key = KeyMapping[action];

                // don't want to trigger these actions if they're currently typing
                // in a text box
                if (IsTextTyping && isSupportedTextTypingKey(key)) return false;

                return (!LastKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyDown(key));
            }

            return false;

        }

        public static bool IsKeyUp(InputAction action)
        {
            // Check gamepad first, if doesn't match that, then try
            // keyboard input
            if(GamePadCapabilities.GamePadType == GamePadType.GamePad)
            {
                // Check for thumbstick direction matching the action requested first, and return true
                // if they are pressed, otherwise check the buttons
                if (isThumbstickPressed(action))
                {
                    return true;
                }

                if(isButtonReleased(action))
                {
                    return true;
                }
            }

            if (KeyMapping.ContainsKey(action))
            {
                var key = KeyMapping[action];

                // don't want to trigger these actions if they're currently typing
                // in a text box
                if (IsTextTyping && isSupportedTextTypingKey(key)) return false;

                return (LastKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key));
            }

            return false;
        }

        private static bool isSupportedTextTypingKey(Keys key)
        {
            return (key >= Keys.A && key <= Keys.Z) || (key >= Keys.D0 && key <= Keys.D9) || key == Keys.Back;
        }

        private static bool isButtonReleased(InputAction action)
        {
            return isButtonAction(action, (Buttons button) => CurrentGamePadState.IsButtonUp(button) && LastGamePadState.IsButtonDown(button));
        }

        private static bool isButtonPressed(InputAction action)
        {
            return isButtonAction(action, (Buttons button) => CurrentGamePadState.IsButtonDown(button) && !LastGamePadState.IsButtonDown(button));
        }

        private static bool isButtonPressing(InputAction action)
        {
            return isButtonAction(action, (Buttons button) => CurrentGamePadState.IsButtonDown(button));
        }


        private static bool isButtonAction(InputAction action, Func<Buttons, bool> checkButton)
        {
            switch (action)
            {
                case InputAction.Up:
                    return checkButton(Buttons.DPadUp);
                case InputAction.Down:
                    return checkButton(Buttons.DPadDown);
                case InputAction.Left:
                    return checkButton(Buttons.DPadLeft);
                case InputAction.Right:
                    return checkButton(Buttons.DPadRight);
                case InputAction.Accept:
                    return checkButton(Buttons.A);
                case InputAction.Back:
                    return checkButton(Buttons.B);
                case InputAction.SpecialAction1:
                    return checkButton(Buttons.X);
                case InputAction.SpecialAction2:
                    return checkButton(Buttons.Y);
                case InputAction.SpecialAction3:
                    return checkButton(Buttons.LeftTrigger);
                case InputAction.SpecialAction4:
                    return checkButton(Buttons.RightTrigger);
                case InputAction.LeftShoulder:
                    return checkButton(Buttons.LeftShoulder);
                case InputAction.RightShoulder:
                    return checkButton(Buttons.RightShoulder);
                case InputAction.Start:
                    return checkButton(Buttons.Start);
                default:
                    return false;
            }
        }

        private static bool isThumbstickPressing(InputAction action)
        {
            if (GamePadCapabilities.HasLeftXThumbStick)
            {
                if (action == InputAction.Left)
                {
                    return CurrentGamePadState.ThumbSticks.Left.X < -0.5f;
                }
                else if (action == InputAction.Right)
                {
                    return CurrentGamePadState.ThumbSticks.Left.X > 0.5f;
                }
            }

            if (GamePadCapabilities.HasLeftYThumbStick)
            {
                if (action == InputAction.Up)
                {
                    return CurrentGamePadState.ThumbSticks.Left.Y > 0.5f;
                }
                else if (action == InputAction.Down)
                {
                    return CurrentGamePadState.ThumbSticks.Left.Y < -0.5f;
                }
            }

            return false;
        }

        // TODO: Keep track of how long time pressed, and periodically treat is as a triggered direction while being held (long for the first one, then shorter for those after so it cycles fairly quick)
        private static bool isThumbstickPressed(InputAction action)
        {
            if(GamePadCapabilities.HasLeftXThumbStick)
            {
                if (action == InputAction.Left)
                {
                    return !(CurrentGamePadState.ThumbSticks.Left.X < -0.5f) && (LastGamePadState.ThumbSticks.Left.X < -0.5f);
                }
                else if(action == InputAction.Right)
                {
                    return !(CurrentGamePadState.ThumbSticks.Left.X > 0.5f) && (LastGamePadState.ThumbSticks.Left.X > 0.5f);
                }
            }

            if (GamePadCapabilities.HasLeftYThumbStick)
            {
                if (action == InputAction.Up)
                {
                    return !(CurrentGamePadState.ThumbSticks.Left.Y > 0.5f) && (LastGamePadState.ThumbSticks.Left.Y > 0.5f);
                }
                else if (action == InputAction.Down)
                {
                    return !(CurrentGamePadState.ThumbSticks.Left.Y < -0.5f) && (LastGamePadState.ThumbSticks.Left.Y < -0.5f);
                }
            }

            return false;
        }
   }
}
