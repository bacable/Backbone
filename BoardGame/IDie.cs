using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.BoardGame
{
    public interface IDie
    {
        string Name { get; set; }
        IZone Owner { get; set; }
        int numberOfFaces { get; set; }
        
    }
}
