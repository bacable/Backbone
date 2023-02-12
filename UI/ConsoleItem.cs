using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.UI
{
    public class ConsoleItem
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public string Message { get; set; }
        public string User { get; set; }
        public Color UserColor { get; set; }
        internal float FadeInAmount { get; set; } = 0f;
        internal float FadeOutAmount { get; set; } = 0f;
    }
}
