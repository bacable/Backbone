using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.BoardGame
{
    public interface ICard : IResource
    {

        void Play();
        void Discard();

    }
}
