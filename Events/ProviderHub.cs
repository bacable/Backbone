using System;
using System.Collections.Generic;
using System.Linq;

namespace Backbone.Events
{
    public class ProviderHub<TReturn,TRequest>
    {
        private static List<IProvider<TReturn, TRequest>> Providers = new List<IProvider<TReturn, TRequest>>();

        public static void RegisterProvider(IProvider<TReturn,TRequest> provider)
        {
            if(!Providers.Any(x => x == provider))
            {
                Providers.Add(provider);
            }
        }

        public static TReturn Request(TRequest infoType, int id = 0)
        {
#if DEBUG
            if (Providers.Count == 0)
            {
                throw new ArgumentException("provider missing for " + typeof(TRequest));
            }
#endif
            for(var i = 0; i < Providers.Count; i++)
            {
                var provider = Providers[i];
                var returnVal = provider.Provide(infoType, id);
                if(!EqualityComparer<TReturn>.Default.Equals(returnVal, default(TReturn))) {
                    return returnVal;
                }
            }

            return default(TReturn);
        }
    }
}
