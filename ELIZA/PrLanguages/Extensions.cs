using System;
using Diggins.Jigsaw;

namespace PrLanguages
{
    public static class Extensions
    {
        public static T Convert<T>(this object obj)
        {
            if (obj == null)
                return default(T);
            return (T)System.Convert.ChangeType(obj, typeof(T));
        }

        public static dynamic Convert(this object obj, Type type)
        {
            return System.Convert.ChangeType(obj, type);
        }

        public static bool ExactMatch(this Rule rule, string line)
        {
            if (rule.Match(line) == false) return false;
            Node n = rule.Parse(line)[0];
            return n.Text == line;
        }

        public static T[] Convert<T>(this dynamic[] arr)
        {
            T[] result = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                result[i] = (T)arr[i];
            return result;
        }
    }
}
