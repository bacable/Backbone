using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximityND.Backbone.Events
{
    public interface IProvider<TReturn,TRequest>
    {
        /// <summary>
        /// Request for a type of information, provided as another type.
        /// </summary>
        /// <param name="infoType">What type of information requested, probably as an enum</param>
        /// <returns>The data requested, can be any type.</returns>
        TReturn Provide(TRequest infoType);
    }
}
