using System;
using System.Linq;

namespace ELIZA.Morphology
{
    [Serializable]
    public class RuPorterStemmer : IStemmer
    {
        //Первая группа перфектных окончаний
        private string[] perfGroupOne = new string[] { "вшись", "вши", "в" };
        //Вторая группы перфектных окончаний
        private string[] perfGroupTwo = new string[] { "ывшись", "ившись", "ывши",
            "ивши", "ыв", "ив" };
        //Группа окончаний прилагательных
        private string[] adj = new string[] { "ими", "ыми", "его", "ого", "ему",
            "ому", "ее", "ие", "ые", "ое", "ей", "ий", "ий",
            "ый", "ой", "ем", "им", "ым", "ом", "их", "ых", "ую",
            "юю", "ая", "яя", "ою", "ею" };
        //Первая группа окончаний причастий
        private string[] partGroupOne = new string[] { "ем", "нн", "вш", "ющ", "щ" };
        //Вторая группа окончаний причастий
        private string[] partGroupTwo = new string[] { "ивш", "ывш", "ующ" };
        //Группа возвратных окончаний
        private string[] reflexive = new string[] { "ся", "сь" };
        //Первая группа окончаний глаголов
        private string[] verbGroupOne = new string[] { "ешь", "нно", "ете", "йте",
            "ла", "на", "ли", "ем", "ло", "но", "ет", "ют", "ны", "ть", "л", "н", "й" };
        //Вторая группы окончаний глаголов
        private string[] verbGroupTwo = new string[] { "ейте", "уйте", "ила", "ыла", "ена",
            "ите", "или", "ыли", "ило", "ыло", "ено", "ует", "уют", "ены", "ить", "ыть",
            "ишь", "ую", "ей", "уй", "ит", "ыт", "ил", "ыл", "им", "ым", "ен", "ят", "ю" };
        //Группа окончаний превосходной степени
        private string[] superlative = new string[] { "ейше", "ейш" };
        //Группа окончаний существительных
        private string[] noun = new string[] { "иями", "ями", "ами", "ией", "иям",
            "ием", "иях", "ям", "ем", "ам", "ом", "ей", "ой",
            "ий", "ах", "ях", "ы", "ию", "ью", "ия",
            "ья", "еи", "ии", "ев", "ов", "ие", "ые",
            "и", "о", "у", "ь", "й", "е", "ю", "а", "я" };
        //Группа дуривативных окончаний
        private string[] derivational = new string[] { "ость", "ост" };
        // Группа гласных букв
        private char[] vowels = new char[] { 'а', 'е', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };
        //Слово для стемминга
        private string word = string.Empty;
        //Номер первого символа области слова после первой гласной
        private int rv = 0;
        //Первый символ области слова после первого сочетания "гласная-согласная"
        private int r1 = 0;
        //Первый символ области rv после первого сочетания "гласная-согласная"
        private int r2 = 0;
        //Номер последнего символа слова
        private int end = 0;
        //Номер последнего символа псевдоосновы
        private int ending = 0;

        Func<int, bool> requires;

        #region IStemmer Members

        public Tuple<string, string> Stem(string word)
        {
            if(word == string.Empty || word == null)
            {
                return new Tuple<string, string>(string.Empty, string.Empty);
            }
            this.word = (string)word.Clone();
            word = word.ToLower();
            word = word.Replace("ё", "е");
            Initialize();
            StepOne();
            StepTwo();
            StepThree();
            StepFour();
            return new Tuple<string, string>(word.Substring(0, ending + 1),
                word.Substring(ending + 1, word.Length - ending - 1));
        }

        #endregion

        private void StepOne()
        {
            if (!RemoveEnding(perfGroupOne, requires))
            {
                if (!RemoveEnding(perfGroupTwo, null)) //за два шага удаляем окончание perfGerung
                {
                    RemoveEnding(reflexive, null); //удаляем Reflexive
                    //удаляет Adjectival = [Participle] + Adjective
                    if (RemoveEnding(adj, null))
                    {
                        if (!RemoveEnding(partGroupOne, requires))
                            RemoveEnding(partGroupTwo, null);
                    }
                    else
                    {
                        //пытаемся удалить Verb
                        if (!(RemoveEnding(verbGroupOne, requires) ||
                            RemoveEnding(verbGroupTwo, null)))
                            RemoveEnding(noun, null); //пытаемся удалить noun
                    }
                }
            }
        }
        private void StepTwo()
        {
            if (RVLetterIs(ending, new char['и'])) ending--;
        }
        private void StepThree()
        {
            if (ending - r2 > 0)
            {
                RemoveEnding(derivational, null);
            }
        }
        private void StepFour()
        {
            if (EndsWith("нн"))
            {
                ending--;
            }
            if (RemoveEnding(superlative, null))
                if (EndsWith("нн"))
                    ending--;
            if (EndsWith("ь")) ending--;
        }
        /// <summary>
        /// Нхаодит необходимые части в слове.
        /// </summary>
        private void Initialize()
        {
            requires = i =>
            {
                return RVLetterIs(i, new char[] { 'а', 'я' });
            };
            end = ending = word.Length - 1;
            rv = r1 = r2 = ending + 1;
            FindRV();
            FindR1();
            FindR2();
        }
        /// <summary>
        /// Находит индекс начала rv.
        /// </summary>
        private void FindRV()
        {
            int i = 0;
            while (i <= end)
            {
                if (vowels.Contains(word[i])) //нашли rv
                {
                    i++;
                    rv = i;
                    return;
                }
                i++;
            }
        }
        /// <summary>
        /// Находит индекс начала r1.
        /// </summary>
        private void FindR1()
        {
            bool vowel = true;
            int i = rv;
            while (i <= end)
            {
                if (vowel) //если предыдущая была гласная
                {
                    if (!vowels.Contains(word[i])) //и текущая - согласная, нашли r1
                    {
                        i++;
                        r1 = i;
                        return;
                    }
                }
                i++;
            }
        }
        /// <summary>
        /// Находит индекс начала r2.
        /// </summary>
        private void FindR2()
        {
            bool vowel = false;
            int i = r1;
            while (i <= end)
            {
                if (!vowel)
                {
                    if (vowels.Contains(word[i])) vowel = true;
                }
                else if (!vowels.Contains(word[i]))
                {
                    r2 = i + 1;
                    return;
                }
                i++;
            }
        }
        /// <summary>
        /// Возвращает true, если на i-той позиции в слове стоит один из символов, заданных
        /// массивом.
        /// </summary>
        /// <param name="i">Позиция в слове.</param>
        /// <param name="arr">Массив символов.</param>
        /// <returns>Возвращает true, если на i-той позиции в слове стоит один из символов, заданных
        /// массивом.</returns>
        private bool RVLetterIs(int i, char[] arr)
        {
            if (i <= rv) return false;
            return arr.Contains(word[i]);
        }
        private bool RemoveEnding(string[] endings, Func<int, bool> condition)
        {
            bool check = (condition != null);
            foreach (string endingL in endings)
            {
                if (EndsWith(endingL))
                {
                    if (check && condition(ending - endingL.Length) || !check)
                    {
                        ending = ending - endingL.Length;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool EndsWith(string endingL)
        {
            if (ending - rv < endingL.Length) return false;
            int j = endingL.Length - 1;
            for (int i = ending; i > ending - endingL.Length; i--)
            {
                if (word[i] != endingL[j]) return false;
                j--;
            }
            return true;
        }
    }
}
