using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Reflection;
using PrLanguages.Grammars;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Содержит вспомогательные методы для обработки морфологии.
    /// </summary>
    public static class Utils
    {
        private static int vectorLenght = Enum.GetValues(typeof(Tag)).Length - 1; //исключаем нулевое значение
        private static Tag maximumEntropyTag = (Tag)Enum.GetValues(typeof(Tag)).Cast<Tag>().Aggregate((Tag)0, (a, x) => a | x, a => (ulong)a);
        private static int maxEndingLenght = 10; //псевдоокончание будет меньше 10 символов
        private const string alphabet = "абвгдеёжзийклмнопрстуфхцчшщьъыэюя"; //алфавит

        /// <summary>
        ///Получает тэг, соответствующий тэгу всех подтэгов.
        /// </summary>
        public static Tag MaximumEntropyTag
        {
            get
            {
                return maximumEntropyTag;
            }
        }
        /// <summary>
        /// Получает максимальную длину тэга.
        /// </summary>
        public static int AttributeVectorLenght
        {
            get { return vectorLenght; }
        }

        /// <summary>
        /// Получает вектор битов, соответствующий заданному <see cref="Tag"/>.
        /// </summary>
        /// <param name="tag">Исходный тэг.</param>
        /// <returns>Возвращает вектор битов, соответствующий заданному <see cref="Tag"/>.</returns>
        public static byte[] ToVector(this Tag tag)
        {
            string s = Convert.ToString((uint)tag, 2);
            return s.PadLeft(vectorLenght, '0').Select(c => byte.Parse(c.ToString())).ToArray();
        }
        public static double[] EncodeEnding(this string word, IStemmer stemmer)
        {
            double[] result = new double[maxEndingLenght];
            if (word != null && word != "")
            {
                string ending = stemmer.Stem(word).Item2;
                for (int i = result.Length - 1, j = ending.Length - 1; i >= 0 && j>= 0; i--, j--) 
                {
                    result[i] = Math.Round((double)(alphabet.IndexOf(ending[j]) + 1) / alphabet.Length, 2);
                }
            }
            return result;
        }
        /// <summary>
        /// Получает <see cref="Tag"/> из ветора.
        /// </summary>
        /// <param name="vector">TИсходный вектор.</param>
        /// <returns>Возвращает <see cref="Tag"/>, соответствующий исходному вектору.</returns>
        public static Tag FromVector(this byte[] vector)
        {
            BitArray bitArray = new BitArray(vector.Select(b => b == 0 ? false : true).ToArray());
            uint[] temp = new uint[1];
            bitArray.CopyTo(temp, 0);
            return (Tag)temp[0];
        }
        /// <summary>
        /// Возвращает перечисление, содержащее n-грамму для каждого слова предложения.
        /// </summary>
        /// <param name="sentence">Предложение.</param>
        /// <param name="n">Количество элементов в n-граммах.</param>
        /// <returns>Возвращает перечисление, содержащее n-грамму для каждого слова предложения.</returns>
        public static IEnumerable<IEnumerable<Tag>> BuildNGramms(this List<WordForm> sentence, int n)
        {
            //для каждого слова построим n-грамму предшествующих слов
            for(int i = 0; i < sentence.Count; i++)
            {
                List<Tag> currNGramm = new List<Tag>();
                //берём n - 1 предыдущих слов и текущее
                for(int j = i - (n - 1); j <= i; j++)
                {                    
                    //если слова нет
                    if (j < 0)
                        currNGramm.Add(Tag.NoWord);
                    else currNGramm.Add(sentence[j].Tag);
                }
                yield return currNGramm;
            }
        }
        /// <summary>
        /// Возвращает перечисление, содержащее окно заданного размера для каждого слова предложения.
        /// </summary>
        /// <param name="sentence">Предложение.</param>
        /// <param name="n">Количество элементов в окне.</param>
        /// <returns>Возвращает перечисление, содержащее окно заданного размера для каждого слова предложения.</returns>
        public static IEnumerable<IEnumerable<WordForm>> BuildAllWindows(this List<string> sentence, int n)
        {
            int half = n / 2;
            //для каждого слова построим окно
            //но не будем включать тэги
            //тэги будут включены при преобразовании в вектор
            for (int i = 0; i < sentence.Count; i++)
            {
                List<WordForm> currNGramm = new List<WordForm>();
                for (int j = i - half; j <= j + half; j++) 
                {
                    //слово за границей предложения
                    if (j < 0 || j >= sentence.Count)
                    {
                        currNGramm.Add(new WordForm("", Tag.NoWord));
                    }
                    else currNGramm.Add(new WordForm(sentence[j],Tag.NoWord));
                }
                yield return currNGramm;
            }
        }
        /// <summary>
        /// Возвращает перечисление, содержащее окно заданного размера для каждого слова предложения.
        /// </summary>
        /// <param name="sentence">Предложение.</param>
        /// <param name="n">Количество элементов в окне.</param>
        /// <returns>Возвращает перечисление, содержащее окно заданного размера для каждого слова предложения.</returns>
        public static IEnumerable<IEnumerable<WordForm>> BuildAllWindows(this List<WordForm> sentence, int n)
        {
            int half = n / 2;
            //для каждого слова построим окно
            for (int i = 0; i < sentence.Count; i++)
            {
                List<WordForm> currNGramm = new List<WordForm>();
                for (int j = i - half; j <= i + half; j++)
                {
                    //слово за границей предложения
                    if (j < 0 || j >= sentence.Count)
                    {
                        currNGramm.Add(new WordForm("", Tag.NoWord));
                    }
                    else currNGramm.Add(sentence[j]);
                }
                yield return currNGramm;
            }
        }
        /// <summary>
        /// Получает вероятность появления заданного тега, после заданной n-граммы.
        /// </summary>
        /// <param name="ngramm">N-граммная модель.</param>
        /// <param name="key">Ключ.</param>
        /// <param name="tag">Исокмый тег.</param>
        /// <returns>Возвращает вероятность появления заданного тега.</returns>
        public static double GetIncludeProbability(this TagNGramm ngramm, IEnumerable<Tag> key, Tag tag)
        {
            ulong occurances = 0;
            IEnumerable<ulong> keyConv = key.Select(a => (ulong)a);
            foreach(var item in ngramm.InnerNgramm.Trie.GetChildNodes(keyConv))
            {
                if(((Tag)item.Key & tag) != 0)
                {
                    occurances += item.Value;
                }
            }
            return occurances / ngramm.Count;
        }
        /// <summary>
        /// Преобразует данные окна в вектор, используя n-граммную модель и модель классов неоднозначности.
        /// </summary>
        /// <param name="window">Окно.</param>
        /// <param name="nGrammModel">N-граммная модель.</param>
        /// <param name="entClassModel">Модель классов неопределённости.</param>
        /// <returns>Возвращает вектор, соответствующий заданному окну.</returns>
        public static double[] ToVector(this IEnumerable<WordForm> window,
            TagNGramm nGrammModel, IEntropyClassModel entClassModel)
        {
            List<WordForm> lexems = window.ToList();
            int targetLexem = lexems.Count / 2; //разгадываемое слово
            int i = 0; //начало следующего вектора
            double[] result = new double[(lexems.Count + 1) * vectorLenght + 11];
            for (int j = 0; j < targetLexem; j++) //первыми идут тэги уже распознанных слов
            {
                byte[] tagVector = lexems[j].Tag.ToVector();
                tagVector.CopyTo(result, i);
                i += tagVector.Length;
            }
            EncodeEnding(lexems[targetLexem].Word, new RuPorterStemmer()).CopyTo(result, i);
            i += 10;
            for (int j = targetLexem; j < lexems.Count; j++) //дальше идут классы неопределённости для всех остальных слов
            {
                byte[] entClassVect;
                if (lexems[targetLexem].Word != "")
                {
                    entClassVect = entClassModel.GetEntropyClass(lexems[targetLexem].Word.ToLower()).ToVector();
                }
                else entClassVect = Tag.NoWord.ToVector();
                entClassVect.CopyTo(result, i);
                i += entClassVect.Length;
            }
            IEnumerable<Tag> key = lexems.Skip(targetLexem - 2).Take(2).Select(a => a.Tag); //ключ для триграммы
            var mostPossible = nGrammModel.GetMostPossibleTag(key); //наиболее вероятный тэг по триграммной модели
            //mostPossible.Item1.ToVector().CopyTo(result, i);
            //i += AttributeVectorLenght + 1;
            //result[i - 1] = Math.Round(mostPossible.Item2, 2); //вероятность наиболее вероятного тэга
            //пропускаем один элемент и получаем ключ для биграммы
            var mostPossible2 = nGrammModel.GetMostPossibleTag(key.Skip(1));
            (mostPossible.Item1 | mostPossible2.Item1).ToVector().CopyTo(result, i);
            i += AttributeVectorLenght + 1;
            result[i - 1] = Math.Round(Math.Max(mostPossible.Item2, mostPossible2.Item2), 2); //вероятность наиболее вероятного тэга
            return result;
        }

        public static List<Lexem> ExtractFormulas(ref string text)
        {
            var result = new List<Lexem>();
            for (var i = 0; i < text.Length; i++)
            {
                var temp = text.Substring(i);
                if (ExpressionsGrammar.Expression.Match(temp))
                {
                    var parsed = ExpressionsGrammar.Expression.Parse(temp)[0];
                    if (parsed.Descendants.Count() > 1)
                    {
                        text = text.Remove(i, parsed.Length);
                        var lexem = new Lexem(0, parsed.Text, Tag.Phantom);
                        lexem.Lemma = parsed.Text;
                        result.Add(lexem);
                    }
                }
            }
            return result;
        }

        public static string ToString(this Tag tag, CultureInfo info, bool t)
        {
            var result = string.Empty;
            var type = typeof (Tag);
            foreach (var i in Enum.GetValues(typeof (Tag)))
            {
                var localisationFound = false;
                if ((tag & (Tag)i) != 0) //в перечислении установлен флаг
                {
                    var memberInfo = type.GetMember(i.ToString())[0];
                    var localisations = memberInfo.GetCustomAttributes<Localisation>();
                    if (localisations != null) // существуют какие-то переводы
                    {
                        foreach (var localisation in localisations)
                        {
                            //язык совпадает с языком перевода
                            if (localisation.CultureInfo.Equals(info)) 
                            {
                                result += localisation.Name + ", ";
                                localisationFound = true; //перевод найден
                                break;
                            }
                        }
                    }
                    if (!localisationFound)
                        result += " " + i.ToString() + " ,";
                }                
            }
            return result.Substring(0, result.Length - 2);
        }
    }
}
