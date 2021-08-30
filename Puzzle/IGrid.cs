using BackBone.Puzzle;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Puzzle
{
    public interface IGrid
    {
        Dictionary<(int row, int col), IPuzzleElement> Grid { get; set; }

        IPuzzleElement ElementAt(int row, int col);
    }
}
