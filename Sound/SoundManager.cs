using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Backbone.Sound
{
    public static class SoundManager<T>
    {

        public static bool ShouldPlay = false;

        private static Dictionary<T, SoundEffect> sounds = new Dictionary<T, SoundEffect>();

        private static string baseFolder = string.Empty;

        private static ContentManager contentManager = null;

        public static void Initialize(ContentManager contentManager, string baseFolder)
        {
            SoundManager<T>.contentManager = contentManager;
            SoundManager<T>.baseFolder = baseFolder;
        }

        public static void Load(T soundEffectType, string path)
        {
            if (contentManager == null) throw new NullReferenceException("Cannot load sound. Content field cannot be null in SoundManager");

            var soundPath = (string.IsNullOrWhiteSpace(baseFolder)) ? path : baseFolder + "\\" + path;

            var soundEffect = contentManager.Load<SoundEffect>(soundPath);

            sounds[soundEffectType] = soundEffect;
        }

        public static void Play(T effectType)
        {
            if (!ShouldPlay) return;

            SoundEffect effect = sounds[effectType];

            if(effect != null)
            {
                effect.Play();
            }
        }
    }
}
