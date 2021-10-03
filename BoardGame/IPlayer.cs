﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.BoardGame
{
    public interface IPlayer : IZone
    {
        List<IZone> Zones { get; set; }
    }
}
