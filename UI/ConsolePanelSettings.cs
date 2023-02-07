using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.UI
{
    public class ConsolePanelSettings
    {
        public Rectangle Bounds { get; set; }
        public int MaxLines { get; set; }
        public float TimeBeforeLineExpiration { get; set; }
    }
}
