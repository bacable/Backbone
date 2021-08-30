using Backbone.Graphics;
using Backbone.Sound;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Actions
{
    internal class PlaySoundAction<T> : IAction3D
    {
        public List<IAction3D> SubActions { get; set; }

        private T soundToPlay;

        public PlaySoundAction(T soundToPlay)
        {
            this.soundToPlay = soundToPlay;
        }

        public void Reset()
        {
        }

        public bool Update(Movable3D movable, GameTime gameTime)
        {
            SoundManager<T>.Play(soundToPlay);
            return true;
        }
    }
}
