using System;
using System.Collections.Generic;
using System.Linq;

namespace Backbone.Input
{
    public static class EnumHelper<T> where T : struct, IComparable, IConvertible
    {
        public static int MaxValue()
        {
            var maxValue = Enum.GetValues(typeof(T)).Cast<T>().Max();
            return Convert.ToInt32(maxValue);
        }

        public static int Count()
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        public static IEnumerable<T> GetAll()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T FromString(string value)
        {
            T result = default;
            Enum.TryParse(value, out result);
            return result;
        }
    }
}
