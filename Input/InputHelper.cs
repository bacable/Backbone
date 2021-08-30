using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Backbone.Input
{
    public class InputHelper
    {
        public static KeyboardState LastKeyboardState;
        public static KeyboardState CurrentKeyboardState;

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
        }

        public static void UpdateAfter()
        {
            LastKeyboardState = CurrentKeyboardState;
        }

        public static bool IsKeyUp(InputAction action)
        {
            if(KeyMapping.ContainsKey(action))
            {
                var key = KeyMapping[action];
                return (LastKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key));
            }

            return false;
        }
   }
}
