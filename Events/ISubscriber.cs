using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Events
{
    public interface ISubscriber<T>
    {
        void HandleEvent(T eventType, object payload);
    }
}
