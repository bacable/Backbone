using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.RPG
{
    public class Item<T,U>
    {
        public T Category { get; set; }
        public U Type { get; set; }

    }
}
