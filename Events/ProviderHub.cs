using System;
using System.Collections.Generic;

namespace ProximityND.Backbone.Events
{
    public class ProviderHub<TReturn,TRequest>
    {
        private static IProvider<TReturn, TRequest> Provider = null;

        public static void RegisterProvider(IProvider<TReturn,TRequest> provider)
        {
            Provider = provider;
        }

        public static TReturn Request(TRequest infoType)
        {
#if DEBUG
            if (Provider == null)
            {
                throw new ArgumentException("provider missing for " + typeof(TRequest));
            }
#endif
            return Provider.Provide(infoType);
        }
    }
}
