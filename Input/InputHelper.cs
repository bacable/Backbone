using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Backbone.Input
{
    public class InputHelper
    {
        public static KeyboardState LastKeyboardState;
        public static KeyboardState CurrentKeyboardState;

        public static GamePadState CurrentGamePadState;
        public static GamePadState LastGamePadState;

        public static GamePadCapabilities GamePadCapabilities;

        public static Dictionary<InputAction, Keys> KeyMapping = new Dictionary<InputAction, Keys>()
        {
            { InputAction.Accept, Keys.Space },
            { InputAction.Back, Keys.Back },
            { InputAction.Left, Keys.Left },
            { InputAction.Right, Keys.Right },
            { InputAction.Up, Keys.Up },
            { InputAction.Down, Keys.Down },
            { InputAction.Select1, Keys.NumPad1 },
            { InputAction.Select2, Keys.NumPad2 },
            { InputAction.Select3, Keys.NumPad3 },
            { InputAction.Select4, Keys.NumPad4 },
            { InputAction.Select5, Keys.NumPad5 },
            { InputAction.SpecialAction1, Keys.Z },
            { InputAction.SpecialAction2, Keys.X },
            { InputAction.SpecialAction3, Keys.C },
            { InputAction.SpecialAction4, Keys.V }
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
                return (LastKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key));
            }

            return false;
        }

        private static bool isButtonReleased(InputAction action)
        {
            switch (action)
            {
                case InputAction.Up:
                    return isButtonReleased(Buttons.DPadUp);
                case InputAction.Down:
                    return isButtonReleased(Buttons.DPadDown);
                case InputAction.Left:
                    return isButtonReleased(Buttons.DPadLeft);
                case InputAction.Right:
                    return isButtonReleased(Buttons.DPadRight);
                case InputAction.Accept:
                    return isButtonReleased(Buttons.A);
                case InputAction.Back:
                    return isButtonReleased(Buttons.B);
                case InputAction.SpecialAction1:
                    return isButtonReleased(Buttons.X);
                case InputAction.SpecialAction2:
                    return isButtonReleased(Buttons.Y);
                case InputAction.SpecialAction3:
                    return isButtonReleased(Buttons.LeftTrigger);
                case InputAction.SpecialAction4:
                    return isButtonReleased(Buttons.RightTrigger);
                case InputAction.Start:
                    return isButtonReleased(Buttons.Start);
                default:
                    return false;
            }
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

        private static bool isButtonReleased(Buttons button)
        {
            return CurrentGamePadState.IsButtonUp(button) && LastGamePadState.IsButtonDown(button);
        }
   }
}
