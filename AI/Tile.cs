using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.AI
{
    public class Tile
    {
        public string ID { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public object Get(string key) { return Properties[key]; }
        public void Set(string key, object value) { Properties[key] = value; }
    }
}
