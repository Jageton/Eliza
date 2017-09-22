using System;
using System.Collections.Generic;
using OGESolver;

namespace ELIZA.Semantics.Converters
{
    /// <summary>
    /// Содержит методы для конвертирования строк в объекты заданого типа.
    /// </summary>
    public static class ConverterFactory
    {
        private static Dictionary<Type, IConverter<object>> converters;

        /// <summary>
        /// Получает набор существующих конвертеров <see cref="IConverter{T}"/>.
        /// </summary>
        public static Dictionary<Type, IConverter<object>> Converters
        {
            get { return converters; }
        }

        static ConverterFactory()
        {
            converters = new Dictionary<Type, IConverter<object>>();
            converters.Add(typeof(ReferenceOf<int>), new IntegerConverter());
            converters.Add(typeof(ReferenceOf<TimeUnit>), new TimeUnitConverter());
            converters.Add(typeof(ReferenceOf<InformationUnit>), new InformationUnitConverter());
        }

        /// <summary>
        /// Пытается конвертировать строку в объект с заданным типом.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="value">Строковое значение.</param>
        /// <returns></returns>
        public static object Convert<T>(string value)
        {
            var type = typeof (T);
            if (converters.ContainsKey(type))
            {
                if (value.Length > 0)
                    return converters[type].Convert(value);
                else return default(T);
            }
            return value;
        }
    }
}
