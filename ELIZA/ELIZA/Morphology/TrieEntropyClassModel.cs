using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Модель классов неоднозначности.
    /// </summary>
    [Serializable]
    [ProtoContract]
    public class TrieEntropyClassModel: TagTrie, IEntropyClassModel
    {
        /// <summary>
        ///внутренний массив, хранящий все значения, встретившиеся модели
        ///уникальные элементы типа Tag помещаются в этот массив
        ///во внутреннее дерево помещается индекс элемента в этом массиве
        ///при получении значения из дерева, полученное значение является индексом этого массива
        /// </summary>
        [ProtoMember(1)]
        protected List<ulong> valueArray;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TrieEntropyClassModel"/>.
        /// </summary>
        public TrieEntropyClassModel(): base()
        {
            this.valueArray = new List<ulong>();
        }

        /// <summary>
        /// Добавляет лексему в модель.
        /// </summary>
        /// <param name="lexem">Лексема.</param>
        public void AddLexem(WordForm lexem)
        {
            IEnumerable<string> key = lexem.Word.ToCharArray().Select(c => c.ToString()); //слоформа является ключём
            if (!valueArray.Contains((ulong)lexem.Tag))
                valueArray.Add((ulong)lexem.Tag);
            if(Contains(key)) //словоформа уже существует
            {
                Tag tag = (Tag)valueArray[Get(key)]; 
                tag |= lexem.Tag; //обновляем класс неоднозначности
                if (!valueArray.Contains((ulong)tag)) //если он ещё не встречался
                    valueArray.Add((ulong)tag);
                Set(key, (ushort)valueArray.IndexOf((ulong)tag));
            }
            else
            {
                //новая словоформа
                Set(key, (ushort)valueArray.IndexOf((ulong)lexem.Tag));
            }
        }
        /// <summary>
        /// Возвращает класс неопределённости для заданного слова. Если слово отсутствует в модели, то возвращает 
        /// максимальный класс.
        /// </summary>
        /// <param name="word">Слово.</param>
        /// <returns>Возвращает класс неопределённости для заданного слова. Если слово отсутствует в модели, то возвращает 
        /// максимальный класс.</returns>
        public Tag GetEntropyClass(string word)
        {
            IEnumerable<string> key = word.ToCharArray().Select(a => a.ToString());
            //если элемент содержится в дереве, то получаем индекс результата и возвращаем
            //элемент внутреннего массива с этим индексом
            //иначе возвращаем максимальный класс энтропии
            Tag result = Contains(key) ? (Tag)valueArray[Get(key)] : Utils.MaximumEntropyTag;
            return result;
        }
        /// <summary>
        /// Сохраняет модель в заданный поток.
        /// </summary>
        /// <param name="fs">Поток.</param>
        public override void SaveTo(Stream fs)
        {
            base.SaveTo(fs);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, valueArray);
        }
        /// <summary>
        /// Загружает модель из заданого потока.
        /// </summary>
        /// <param name="fs">Поток.</param>
        public override void Load(Stream fs)
        {
            base.Load(fs);
            BinaryFormatter bf = new BinaryFormatter();
            valueArray = (List<ulong>)bf.Deserialize(fs);
        }
    }
}
