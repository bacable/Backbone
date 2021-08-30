using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.BoardGame
{
    public interface IGame
    {

        List<IZone> SharedZones { get; set; }
        List<IPlayer> Players {get; set; }

        void Setup();
        void NextTurn();
        void CheckForGameOver();
        void DetermineWinner();
    }
}
