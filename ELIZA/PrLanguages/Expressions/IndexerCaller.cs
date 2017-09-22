using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PrLanguages.Expressions
{
    public static class IndexerCaller
    {
        public static dynamic CallIndexerValue(dynamic target, dynamic[] args)
        {
            Type type = target.GetType();
            if(target is Array)
            {
                MethodInfo method = type.GetMethod("GetValue", new Type[] { typeof(int[]) });
                return method.Invoke(target, new object[] { args.Convert<int>() });
            }

            IEnumerable<PropertyInfo> properties =
                type.GetProperties().Where<PropertyInfo>(
                (a) => {return a.GetIndexParameters().Length == args.Length; });
            return properties.First().GetValue(target, args);
        }

        public static void SetIndexerValue(dynamic target, dynamic[] args, dynamic value)
        {
            Type type = target.GetType();
            if (target is Array)
            {
                Type t = ((Array)target).GetType().GetElementType();
                MethodInfo method = type.GetMethod("SetValue", new Type[] { typeof(int[]),  t});
                method.Invoke(target, new object[] { value, args.Convert<int>() });
            }

            IEnumerable<PropertyInfo> properties =
                type.GetProperties().Where<PropertyInfo>(
                (a) => { return a.GetIndexParameters().Length == args.Length; });
            PropertyInfo indexer = properties.First();
            indexer.SetValue(target, value, args);
        }
    }
}
