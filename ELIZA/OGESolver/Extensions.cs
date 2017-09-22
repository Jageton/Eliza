using System;
using System.Collections.Generic;
using System.Linq;

namespace OGESolver
{
    public static class Extensions
    {
        public static int GetNotificationValue(this char c)
        {
            if (char.IsDigit(c)) return int.Parse(c.ToString());
            return (c - 'A' + 10);
        }
        public static char GetCharValue(this int numb)
        {
            if (numb < 10) return (numb).ToString()[0];
            return ((char)(numb - 10 + 'A'));
        }
        public static List<T> Convert<T>(this List<string> list, Func<string, T> converter)
        {
            List<T> result = new List<T>();
            foreach(var item in list)
            {
                result.Add(converter(item));
            }
            return result;
        }
        public static T[] Convert<T>(this string[] array, Func<string, T> converter)
        {
            return array.ToList().Convert<T>(converter).ToArray();
        }
        
    }
}
