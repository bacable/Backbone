﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.BoardGame
{
    public interface IResource : IGameComponent
    {
        /// <summary>
        /// Copies this game component and returns a copy.
        /// </summary>
        /// <returns></returns>
        IResource Copy();
    }
}
