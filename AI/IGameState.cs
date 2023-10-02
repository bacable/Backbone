using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximityND.Backbone.AI
{
    public interface IGameState
    {
        IGameState ApplyMove(IMoveState move);
    }
}
