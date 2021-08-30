using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Puzzle
{
    /// <summary>
    /// Use this to keep track of which puzzles each player has solved
    /// </summary>
    public class PuzzleStatus
    {
        public IPuzzle Puzzle { get; set; }
        public bool HasSolved { get; set; } = false;

    }
}
