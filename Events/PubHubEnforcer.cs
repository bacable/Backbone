using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backbone.Events
{
    public static class PubHubEnforcer<T>
    {
        public static List<string> GetAllSubscribers()
        {
            var subscribers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(ISubscriber<T>).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .Select(x => x.Name).ToList();
            return subscribers;
        }
    }
}
