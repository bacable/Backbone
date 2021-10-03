using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.BoardGame
{
    public interface ICard : IResource
    {
        string Name { get; set; }
        string Owner { get; set; }
        string CardType { get; set; }
        int Rank { get; set; }
        int Value { get; set; }
        int Suit { get; set; }

        void Play();
        void Discard();

    }
}
