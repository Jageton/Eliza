using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ELIZA.Semantics
{
    /// <summary>
    /// Задаёт множество синонимичных понятий в данной области.
    /// </summary>
    public static class Synonims
    {
        private static Dictionary<string, string> dict;

        static Synonims()
        {
            dict = new Dictionary<string, string>();
            dict.Add("двоичный", "2");
            dict.Add("десятичный", "10");
            dict.Add("троичный", "3");
        }

        /// <summary>
        /// Получает определённый общий синоним для заданного слова. Если такого синонима нет, 
        /// возвращает само слово.
        /// </summary>
        /// <param name="word">Слово.</param>
        /// <returns>Возвращает общйи синоним для заданного слова.</returns>
        public static string Get(string word)
        {
            if (dict.ContainsKey(word))
            {
                return dict[word];
            }
            return word;
        }

        /// <summary>
        /// Инициализирует словарь синонимов, используя текстовый файл.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        public static void InitializeFromFile(string fileName)
        {
            dict = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
            using (TextReader tr = sr)
            {
                var line = string.Empty;
                while ((line = tr.ReadLine()) != null)
                {
                    var splitted = line.Split(new char[] {' '},
                        StringSplitOptions.RemoveEmptyEntries);
                    for (var i = 1; i < splitted.Length; i++)
                    {
                        dict.Add(splitted[i], splitted[0]);
                    }
                }
            }
        }
    }
}
