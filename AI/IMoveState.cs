using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximityND.Backbone.AI
{
    public interface IMoveState
    {
        IGameState GameState { get; set; }
        object data { get; set; }
        float Score { get; set; }
    }
}
