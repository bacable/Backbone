using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.BoardGame
{
    public class GameDefinition
    {
        public string Name { get; set; }
        public int PlayerCount { get; set; }
        public string GameMode { get; set; }
        public List<IGameComponent> Components { get; set; }
        public PhaseEngine Phases { get; set; }
    }
}
