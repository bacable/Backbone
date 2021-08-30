using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Puzzle
{
    public interface IPuzzle
    {
        public string Id { get; set; }
        public IReward Reward { get; set; }
        public int DifficultyLevel { get; set; }
        public ISolution Solution { get; set; }
        public string Data { get; set; }
        public int Order { get; set; }
    }
}
