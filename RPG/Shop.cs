using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.RPG
{
    class Shop<T,U>
    {
        public List<(Item<T, U> item, int quantity)> Items { get; set; } = new List<(Item<T, U> item, int quantity)>();
    }
}
