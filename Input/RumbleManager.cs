using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Backbone.Input
{
    public class RumbleManager
    {
        private static Dictionary<PlayerIndex, RumbleEffect> _rumbleEffects = new Dictionary<PlayerIndex, RumbleEffect>();
        public static RumbleSetting Setting { get; set; } = RumbleSetting.GameMenu;

        public static bool IsActive { get; set; } = true;

        public static void Update(GameTime gameTime)
        {
            List<PlayerIndex> toRemove = new List<PlayerIndex>();

            foreach (var effect in _rumbleEffects)
            {
                effect.Value.Update(gameTime);

                if (effect.Value.IsFinished)
                {
                    GamePad.SetVibration(effect.Key, 0f, 0f);
                    toRemove.Add(effect.Key);
                }
            }

            foreach (var playerIndex in toRemove)
            {
                _rumbleEffects.Remove(playerIndex);
            }
        }

        public static void Rumble(PlayerIndex playerIndex, float leftMotor, float rightMotor, float durationInSeconds, string category)
        {
            if (!ShouldRumble(category)) return;

            _rumbleEffects[playerIndex] = new RumbleEffect(leftMotor, rightMotor, durationInSeconds);
            GamePad.SetVibration(playerIndex, leftMotor, rightMotor);
        }

        public static bool ShouldRumble(string category)
        {
            var categoryType = EnumHelper<RumbleCategory>.FromString(category);

            if (!IsActive) return false;

            switch(categoryType)
            {
                case RumbleCategory.Game:
                    return Setting == RumbleSetting.Game || Setting == RumbleSetting.GameMenu;
                case RumbleCategory.Menu:
                    return Setting == RumbleSetting.Menu || Setting == RumbleSetting.GameMenu;
                default:
                    return false;
            }
        }

        private class RumbleEffect
        {
            public float LeftMotor { get; }
            public float RightMotor { get; }
            public float Duration { get; }
            public bool IsFinished { get; private set; }
            private float _elapsedTime;

            public RumbleEffect(float leftMotor, float rightMotor, float duration)
            {
                LeftMotor = leftMotor;
                RightMotor = rightMotor;
                Duration = duration;
                IsFinished = false;
                _elapsedTime = 0;
            }

            public void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime >= Duration)
                {
                    IsFinished = true;
                }
            }
        }
    }
}