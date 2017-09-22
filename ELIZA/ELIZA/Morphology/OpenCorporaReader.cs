using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Парсер модели OpenCorpora <see cref="http://www.opencorpora.org/dict.php?act=gram"/>.
    /// </summary>
    /// <seealso cref="ELIZA.Morphology.ICorporaReader" />
    [Serializable]
    public class OpenCorporaReader: ICorporaReader
    {
        protected XmlReader reader;
        protected Dictionary<string, Tag> attributes;
        protected string[] exclude = {"Abbr", "Init"};

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="OpenCorporaReader"/>.
        /// </summary>
        public OpenCorporaReader()
        {
            FillAttributes();
        }

        /// <summary>
        /// Заполняет список распознаваемых аттрибутов.
        /// </summary>
        /// <param name="type">Тип перечисления, содержащего аттрибут.</param>
        private void FillAttributes()
        {
            attributes = new Dictionary<string, Tag>();
            foreach(var item in Enum.GetValues(typeof(Tag)))
            {
                Enum value = (Enum)item;
                var memberInfo = typeof(Tag).GetField(value.ToString());
                if (memberInfo.GetCustomAttribute<OpenCorporaName>() != null)
                {
                    string name = memberInfo.GetCustomAttribute<OpenCorporaName>().Name;
                    attributes.Add(name, (Tag)value);
                }
            }            
        }

        #region ICorporaReader Members

        /// <summary>
        /// Читает заданное количество предложений из открытого файла.
        /// </summary>
        /// <param name="amount">Количество предложений..</param>
        /// <returns>
        /// Возвращает перечисление, содержащее списки лексем для каждого предложения.
        /// </returns>
        public IEnumerable<List<WordForm>> ReadSentences(int amount)
        {
            int i = 0;
            foreach (XElement sentence in reader.GetAllElements("sentence"))
            {
                if (i == amount) break;
                yield return ReadOneSentence(sentence);
                i++;
            }
        }
        /// <summary>
        /// Читает одно предложение из заданного элемента.
        /// </summary>
        /// <param name="element">XML элемент, содержащий предложение.</param>
        /// <returns>
        /// Возвращает список лексем прочитанного предложения.
        /// </returns>
        public List<WordForm> ReadOneSentence(XElement element)
        {
            XElement tokens = element.Element("tokens");
            List<WordForm> lexems = new List<WordForm>();
            foreach (XElement token in tokens.Elements("token"))
            {
                WordForm lexem = new WordForm();
                XElement tfr = token.Element("tfr");
                XElement v = tfr.Element("v");
                XElement l = v.Element("l");
                XElement[] att = l.Elements("g").ToArray();
                lexem.Word = token.Attribute("text").Value.ToLower();
                Tag tag = Tag.NoWord;
                for (int i = 0; i < att.Length; i++)
                {
                    Tag currentSubtag;
                    string attrValue = att[i].Attribute("v").Value;
                    if(attributes.TryGetValue(attrValue, out currentSubtag))
                    {
                        tag |= currentSubtag;
                    }
                }
                lexem.Tag = tag;
                lexems.Add(lexem);
            }
            return lexems;
        }
        /// <summary>
        /// OОткрывает файл с заданным именем.
        /// </summary>
        /// <param name="fileName">имя файла.</param>
        public void Open(string fileName)
        {
            reader = XmlReader.Create(fileName);
        }
        /// <summary>
        /// Закрывает все открытые файлы.
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }
        /// <summary>
        /// Читает из исходного файла-словаря заданной количество словоформ.
        /// </summary>
        /// <param name="wordCount">Количество слов.</param>
        /// <returns>
        /// Возвращает перечисление, содержащее прочитанные слова.
        /// </returns>
        public IEnumerable<WordForm> ReadDictionary(long wordCount)
        {
            long i = 0;
            foreach (XElement lemmata in reader.GetAllElements("lemma"))
            {
                Tag commonTag = Tag.NoWord;
                foreach (XElement attr in lemmata.Element("l").Elements("g"))
                {
                    if (attributes.ContainsKey(attr.Attribute("v").Value))
                        commonTag |= attributes[attr.Attribute("v").Value];
                }
                foreach (XElement wordForm in lemmata.Elements("f"))
                {
                    Tag currentTag = commonTag;
                    foreach (XElement attr in wordForm.Elements("g"))
                    {
                        if(exclude.Contains(attr.Value))
                        {
                            currentTag = Tag.NoWord;
                            break;
                        }
                        if (attributes.ContainsKey(attr.Attribute("v").Value))
                            currentTag |= attributes[attr.Attribute("v").Value];
                    }
                    if(currentTag == Tag.NoWord)
                        continue;
                    WordForm lexem = new WordForm(wordForm.Attribute("t").Value.ToLower(), currentTag);
                    if (lexem.Tag != Tag.Unfixed && lexem.Tag != Tag.NoWord)
                    {
                        i++;
                        yield return lexem;
                    }
                    if (i == wordCount) break;
                }
                if (i == wordCount) break;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if(reader != null)
                reader.Close();
        }

        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="OpenCorporaReader"/> class.
        /// </summary>
        ~OpenCorporaReader()
        {
            this.Dispose();
        }
    }
}
