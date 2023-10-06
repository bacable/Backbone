using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximityND.Backbone.AI
{
    public interface IGameState
    {
        float Score { get; set; }
        void ApplyMove(IMoveState move);
    }
}
