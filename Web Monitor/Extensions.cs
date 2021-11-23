using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Monitor
{
    public static class Extensions
    {
        public static T[] InitArr<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }
    }
}
