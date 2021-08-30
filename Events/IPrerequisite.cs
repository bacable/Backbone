using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Events
{
    public interface IPrerequisite<T>
    {
        bool Check(T eventType);
    }
}
