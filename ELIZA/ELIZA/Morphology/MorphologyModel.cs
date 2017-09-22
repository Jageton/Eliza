using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProtoBuf;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using LemmaSharp.Classes;
using System.Security.Permissions;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Морфологическая модель языка.
    /// </summary>
    /// <seealso cref="System.Runtime.Serialization.ISerializable" />
    [Serializable]
    public class MorphologyModel: ISerializable
    {
        private TagNGramm nGramm;
        private IEntropyClassModel entClass;
        private List<TagGroup> groups;
        private string folder;
        private List<string> punctuation;
        private List<string> sentenceDelimiters;
        private int minLengh = 2;
        private string lemmaFile;
        private Lemmatizer lemmatizer;
        //регулярное выражение для разбиения на предложения
        private Regex sentencePattern;
        //регулярное выражение для разбиения на лексемы
        private Regex lexemPattern;
        //тэги служебных частей речи (предлоги, частицы, союзы) 
        private Dictionary<string, Tag> serviceTags; 


        /// <summary>
        /// Инциализирует новый экземпляр класса <see cref="MorphologyModel"/>.
        /// </summary>
        /// <param name="nGramm">N-граммная модель.</param>
        /// <param name="entModel">Модель классов неоднозначности.</param>
        /// <param name="folder">Папка для хранения файлов модели.</param>
        /// <param name="lemmaFile">Файл лемматизатора.</param>
        public MorphologyModel(TagNGramm nGramm, IEntropyClassModel entModel,
            string folder, string lemmaFile)
        {
            this.nGramm = nGramm;
            this.entClass = entModel;
            this.folder = folder;
            if(Directory.Exists(folder))
                Directory.Delete(folder, true);
            Directory.CreateDirectory(folder);
            this.lemmaFile = folder + "\\" + lemmaFile;
            File.Copy(lemmaFile, this.lemmaFile);
            FileStream fs = File.OpenRead(this.lemmaFile);
            lemmatizer = new Lemmatizer(fs);
            Initialize();
        }
        /// <summary>
        /// Инциализирует новый экземпляр класса <see cref="MorphologyModel"/>.
        /// </summary>
        /// <param name="folder">Папка для хранения файлов модели.</param>
        /// <param name="lemmaFile">Файл лемматизатора.</param>
        public MorphologyModel(string folder, string lemmaFile): this(null, null, folder, lemmaFile)
        {

        }
        /// <summary>
        /// Инциализирует новый экземпляр класса <see cref="MorphologyModel"/>.
        /// </summary>
        /// <param name="si">Информация о сериализации.</param>
        /// <param name="context">Контекст.</param>
        protected MorphologyModel(SerializationInfo si, StreamingContext context)
        {
            this.folder = si.GetString("folder");
            this.groups = (List<TagGroup>)si.GetValue("groups", typeof(List<TagGroup>));            
            this.punctuation = (List<string>)si.GetValue("punctuation", typeof(List<string>));
            using(FileStream fs1 = File.Open(string.Format("{0}/{1}", folder, "nGramm.mdl"), FileMode.Open))
            using (FileStream fs2 = File.Open(string.Format("{0}/{1}", folder, "entClass.mdl"), FileMode.Open))            
            {
                this.nGramm = Serializer.Deserialize<TagNGramm>(fs1);
                this.entClass = new DawgEntropyClassModel();
                entClass.Load(fs2);
            }
            this.sentenceDelimiters = (List<string>)si.GetValue("delimiters", typeof(List<string>));
            this.lemmaFile = si.GetString("lemmaFile");
            FileStream fs = File.Open(this.lemmaFile, FileMode.Open);
            this.lemmatizer = new Lemmatizer(fs);
            this.sentencePattern = (Regex)si.GetValue("sentencePattern", typeof(Regex));
            this.lexemPattern = (Regex)si.GetValue("lexemPattern", typeof(Regex));
            this.minLengh = si.GetInt32("minLenght");
            serviceTags = (Dictionary<string, Tag>)si.GetValue("serviceTags",
                typeof (Dictionary<string, Tag>));
        }

        /// <summary>
        /// Запускает процесс обучения модели.
        /// </summary>
        /// <param name="corporaFile">Исходный файл корпуса.</param>
        /// <param name="sentCount">Количество предложений, которые будут прочитаны.</param>
        /// <param name="concurrent"><c>true</c>, если обучение должно происходить 
        /// параллельно (для такого варианта может понадобиться больше памяти, но 
        /// процесс займёт меньше времени).</param>
        /// <remarks>Может занять очень много времени и использовать большое 
        /// количество памяти.</remarks>
        public void Train(string corporaFile, ICorporaReader reader, int sentCount, bool concurrent)
        {
            punctuation = new List<string>();
            sentenceDelimiters = new List<string>();
            reader.Open(corporaFile);
            //читаем заданное количество предложений
            int i = 0;
            foreach (var sentence in reader.ReadSentences(sentCount))
            {
                //запишем все знаки пунктуации, которые встретим
                foreach (var punc in sentence.Where(a => (a.Tag & Tag.Punctuation) != 0 &&
                    !punctuation.Contains(a.Word)))
                    punctuation.Add(punc.Word);
                WordForm last = sentence.Last();
                if ((last.Tag & Tag.Punctuation) != 0 && !sentenceDelimiters.Contains(last.Word))
                    sentenceDelimiters.Add(last.Word);
                if (sentence.Count(lexem => lexem.Tag == Tag.NoWord) == 0)
                {
#if DEBUG
                    i++;
                    Console.Write(string.Format("\rПредложений прочитано: {0}", i));
#endif
                    //строим все возможные окна длины 7
                    foreach (var window in Utils.BuildAllWindows(sentence, 7))
                    {
                        Tag tag = window.Skip(3).First().Tag; //тэг разгадываемого слова
                        foreach (var group in groups) //для каждой группы 
                            group.AddVector(window.ToVector(nGramm, entClass), tag);
                    }
                }
            }
            if (concurrent) //обучаем параллельно
            {
                Parallel.ForEach<TagGroup>(groups, (group) => { group.Train(true); });
            }
            else
            {
                //обучаем последовательно
                i = 0;
                foreach (var group in groups)
                {
                    group.Train(false);
#if DEBUG
                    Console.WriteLine("Закончено обучение группы " + i);
#endif
                    i++;
                }
            }
            string pattern = @"(-?\d+(?:\.\d+)?|";
            foreach (var sign in punctuation)
                pattern += Regex.Escape(sign) + "+|";
            pattern = pattern + @"\s+)";
            this.lexemPattern = new Regex(pattern);
            this.sentencePattern = new Regex(@"(\.+|!|\?)");
            reader.Close();
        }
        /// <summary>
        /// Тренирует модель полностью, включая модель классов энтропии и n-граммную модель.
        /// </summary>
        /// <param name="dictionary">Словарь.</param>
        /// <param name="nGrammCorpora">Корпус для n-граммной модели.</param>
        /// <param name="trainingCorpora">Корпус для обучения модели.</param>
        /// <param name="reader">Объект, осуществляющий чтение корпуса.</param>
        /// <param name="sentCount">Количество предложений для чтения.</param>
        /// <param name="concurrent">Установить <c>true</c>, если необходимо выполнить параллельно. </param>
        /// <param name="callBack">Фукнция для печати прогресса.</param>
        public void TrainFull(string dictionary, string nGrammCorpora, string trainingCorpora,
            ICorporaReader reader, int sentCount, bool concurrent,
            Action<string> callBack = null)
        {
            if (callBack == null)
                callBack = (s) => { };
            callBack("Начато построение модели классов энтропии.");
            BuildEntropyClassModel(dictionary, reader);
            callBack("Построение модели классов энтропии заврешено.");
            callBack("Начато построение n-граммной модели.");
            BuildNGrammModel(nGrammCorpora, reader);
            callBack("Построение n-грамнной модели завершено.");
            callBack("Начато обучение классификаторов.");
            TrainFull(trainingCorpora, reader, sentCount, concurrent);
            callBack("Обучение Завершено.");
        }
        /// <summary>
        /// Трунирует модель классов энтропии.
        /// </summary>
        /// <param name="inputFile">Входной файл словаря.</param>
        /// <param name="reader">Объект для чтения корпуса.</param>
        private void BuildEntropyClassModel(string inputFile, ICorporaReader reader)
        {
            entClass = new DawgEntropyClassModel();
            reader.Open(inputFile);
            serviceTags = new Dictionary<string, Tag>();
            foreach (WordForm lexem in reader.ReadDictionary(long.MaxValue))
            {
                if ((lexem.Tag & (Tag.Conjunction | Tag.Particle | Tag.Preposition)) != 0)
                {
                    Tag outTag = Tag.NoWord;
                    serviceTags.TryGetValue(lexem.Word.ToLower(), out outTag);
                    serviceTags[lexem.Word.ToLower()] = outTag | lexem.Tag;
                }
                entClass.AddLexem(lexem);
            }
            ((DawgEntropyClassModel)entClass).Build();
            reader.Close();
        }
        /// <summary>
        /// Строит n-грамную модель.
        /// </summary>
        /// <param name="inputFile">Входной файл корпуса.</param>
        /// <param name="reader">Объект для чтения корпуса.</param>
        private void BuildNGrammModel(string inputFile, ICorporaReader reader)
        {
            reader.Open(inputFile);
            nGramm = new TagNGramm();
            int nGrammSize = 3;
            foreach (var sentense in reader.ReadSentences(int.MaxValue))
            {
                //предлоги в корпусе не имеют падежа, а союзы не имеют типа
                //чтобы добавить эту информацию, воспользуемся модифицированным словарём
                foreach (var lexem in sentense)
                {
                    if (serviceTags.ContainsKey(lexem.Word.ToLower()))
                        lexem.Tag = serviceTags[lexem.Word];
                }
                if (sentense.Count > 0 && sentense.Count(a => a.Tag == Tag.NoWord ||
                    a.Tag == Tag.Unfixed) == 0)
                {
                    foreach (var ngamm in sentense.BuildNGramms(nGrammSize))
                    {
                        nGramm.AddNGramm(ngamm);
                    }
                }
            }
            reader.Close();
        }
        /// <summary>
        /// Трунирует модель целиком.
        /// </summary>
        /// <param name="corporaFile">Файл корпуса.</param>
        /// <param name="reader">Объект для чтения корпуса.</param>
        /// <param name="sentCount">Количество предложений.</param>
        /// <param name="concurrent">Установить <c>true</c>, если необходимо выполнить параллельно. </param>
        private void TrainFull(string corporaFile, ICorporaReader reader, int sentCount,
            bool concurrent)
        {
            reader.Open(corporaFile);
            punctuation = new List<string>();
            sentenceDelimiters = new List<string>();
            reader.Open(corporaFile);
            //читаем заданное количество предложений
            foreach (var sentence in reader.ReadSentences(sentCount))
            {
                //предлоги в корпусе не имеют падежа, а союзы не имеют типа
                //чтобы добавить эту информацию, воспользуемся модифицированным словарём
                foreach(var lexem in sentence)
                {
                    if (serviceTags.ContainsKey(lexem.Word.ToLower()))
                        lexem.Tag = serviceTags[lexem.Word];
                }
                //запишем все знаки пунктуации, которые встретим
                foreach (var punc in sentence.Where(a => (a.Tag & Tag.Punctuation) != 0 &&
                    !a.Word.Any((c) => char.IsLetter(c)) && !punctuation.Contains(a.Word)))
                    punctuation.Add(punc.Word);
                WordForm last = sentence.Last();
                if ((last.Tag & Tag.Punctuation) != 0 && !sentenceDelimiters.Contains(last.Word))
                    sentenceDelimiters.Add(last.Word);
                if (sentence.Count(lexem => lexem.Tag == Tag.NoWord || lexem.Tag == Tag.Unfixed) == 0)
                {
                    //строим все возможные окна длины 7
                    foreach (var window in Utils.BuildAllWindows(sentence, 7))
                    {
                        Tag tag = window.Skip(3).First().Tag; //тэг разгадываемого слова
                        foreach (var group in groups) //для каждой группы 
                             group.AddVector(window.ToVector(nGramm, entClass), tag);
                    }
                }
            }
            if (concurrent) //обучаем параллельно
                Parallel.ForEach(groups, (group) => { group.Train(true); });
            else
            {
                foreach (var group in groups)
                    group.Train(false);
            }
            string pattern = @"(-?\d+(?:\.\d+)?|";
            foreach (var sign in punctuation)
                pattern += Regex.Escape(sign) + "+|";
            pattern = pattern + @"\s+)";
            this.lexemPattern = new Regex(pattern);
            this.sentencePattern = new Regex(@"(\.+|!|\?)");
            reader.Close();
        }
        /// <summary>
        /// Сопоставляет каждому слову его тэг.
        /// </summary>
        /// <param name="sentence">Исходное предложение.</param>
        /// <returns>Возвращает предложение, где каждому слову поставлен в 
        /// соответствие тэг.</returns>
        /// <remarks>Алгоритм сначала пытается проверить самые простые случаи (
        /// число или знак пунктуации), затем проверяет тэг из словаря. Если тэг 
        /// содержит неоднозначности, то они разрешаются с помощью обученных 
        /// классификаторов.</remarks>
        public List<Lexem> Predict(IEnumerable<string> sentence)
        {
            List<WordForm> result = new List<WordForm>();
            int i = 0;
            foreach (var word in sentence)
            {
                string wordLower = word.ToLower();
                if (punctuation.Contains(wordLower))
                {
                    var n = new Lexem(i, wordLower, Tag.Punctuation);
                    n.Lemma = n.Word;
                    result.Add(n);                    
                }
                else if (serviceTags.ContainsKey(wordLower))
                {
                    var n = new Lexem(i, wordLower, serviceTags[wordLower]);
                    n.Lemma = n.Word;
                    result.Add(n);
                }
                else
                {
                    var n = new Lexem(i,wordLower, entClass.GetEntropyClass(wordLower));
                    n.Lemma = lemmatizer.Lemmatize(n.Word);
                    result.Add(n);
                }
                i++;
            }
            i = 0;
            foreach(var window in result.BuildAllWindows(7))
            {
                double v;
                if (double.TryParse(result[i].Word, out v)) //если число
                    result[i].Tag = Tag.Number;
                else if (punctuation.Contains(result[i].Word)) //если пунктуация
                {
                    result[i].Tag = Tag.Punctuation;
                }
                else //иначе нужно использовать модель
                {

                    //начнём с класса энтропии для данного слова
                    Tag resultingTag = result[i].Tag;
                    //для каждой группы аттрибутов будем пытаться уменьшить класс энтропии
                    foreach(var group in groups) 
                    {
                        //если текущий тэг содержит один или более элементов группы
                        if(group.ContainsGroup(resultingTag)) 
                        {
                            //если текущий тэг содержит более одного элемента группы
                            if(!group.ContainsOneFromGroup(resultingTag))
                            {
                                //преобразуем окно в вектор и угадаем, используя модель                               
                                Tag predictedTag = group.Predict(window.ToVector(nGramm, entClass));
                                resultingTag &= ~group.FullGroup;
                                resultingTag |= predictedTag;                                
                            }
                        }                        
                    }
                    result[i].Tag = resultingTag;
                }             
                i++; //переходим к следующему слову
            }
            var res = new List<Lexem>();
            for (int j = 0; j < result.Count; j++)
            {
                res.Add(new Lexem(j, result[j].Word, result[j].Tag));
                res[j].Lemma = result[j].Lemma;
            }
            return res;
        }
        /// <summary>
        /// Возвращает список предложений, каждое из которых представляет собой список словоформ.
        /// </summary>
        /// <param name="text">Исходный текст.</param>
        /// <returns> Возвращает список предложений,
        /// каждое из которых представляет собой список словоформ</returns>
        public List<List<Lexem>> Predict(string text)
        {
            List<List<Lexem>> result = new List<List<Lexem>>();
            //разбиваем на предложения
            var sentences = SplitIntoSentences(text).ToArray();
            for (var i = 0; i < sentences.Length; i++)
            {
                //вычленяем формулы из предложения
                var formulas = Utils.ExtractFormulas(ref sentences[i]);
                //разрешаем каждое предложение с помощью модели
                var current = Predict(SplitIntoLexems(sentences[i]));
                //добавляем формулы в разобранное предложение
                current.InsertRange(0, formulas);
                result.Add(current);
            }
            return result;
        }
        /// <summary>
        /// Разбивает исходный текст.
        /// </summary>
        /// <param name="text">Исходный текст.</param>
        /// <returns></returns>
        public IEnumerable<string> SplitIntoSentences(string text)
        {
            string[] temp = sentencePattern.Split(text).Where((a) => a != string.Empty).ToArray();
            List<string> result = new List<string>();
            string currSentence = string.Empty;
            for(int i = 0; i < temp.Length; i++)
            {
                currSentence += temp[i];
                //если это разделитель
                if (sentenceDelimiters.Contains(temp[i]))
                {
                    //если это точка, то, возможно, она не разделитель
                    if (temp[i] == ".")
                    {
                        if (i < temp.Length - 1) //если дальше ещё есть текст
                        {
                            string lastLexem = SplitIntoLexems(currSentence.Substring(0, currSentence.Length - 1)).Last(); //последняя лексема
                            string first = SplitIntoLexems(temp[i + 1]).First(); //следующая лексема
                            double v = 0;
                            //если на стыке получилось число, то продолжаем предложение
                            if (double.TryParse(lastLexem + "." + first, out v))
                                continue;
                            //если это не полное слово и не число
                            if(!this.entClass.Contains(lastLexem) && !double.TryParse(lastLexem, out v))
                            {
                                //слово слишком короткое, чтобы заканчивать предложение
                                if (lastLexem.Length <= minLengh)
                                    continue;
                            }
                        }
                    }
                    result.Add(currSentence);
                    currSentence = string.Empty;
                }
            }
            if (currSentence.Length > 0) //было предложение без точки
            {
                currSentence += " .";
                result.Add(currSentence);
            }
            return result;
        }
        /// <summary>
        /// Делит заданное предложение на лексемы.
        /// </summary>
        /// <param name="sentence">.</param>
        /// <returns>Возвращает перечисление, содержащее лексемы данного предложения.</returns>
        public IEnumerable<string> SplitIntoLexems(string sentence)
        {
            return lexemPattern.Split(sentence).
                Where((a) => a != string.Empty && a.All((c)=> c != ' ')).ToArray();
        }
        /// <summary>
        /// Инициализирует текущий объект значениями по умолчанию.
        /// </summary>
        private void Initialize()
        {
            groups = new List<TagGroup>();
            Type t = typeof(Tag);
            var att = t.GetCustomAttribute<MorphologyGroup>();
            if(att != null) //если количество групп указано у перечисления
            {
                int max = att.Number; //максимальное количество групп
                int found = 0; //количество найденных групп
                List<int> foundGroups = new List<int>(); //номера найденных групп
                int i = 0; //номер элемента массива
                Array values = Enum.GetValues(t); //получаем значения перечисления
                while(i < values.GetLength(0) && found < max)
                {
                    Enum value = (Enum)values.GetValue(i);
                    var valueAtt = t.GetField(value.ToString()).GetCustomAttribute<MorphologyGroup>();
                    if(valueAtt != null) //если аттрибут группы указан
                    {
                        //если текущая группа не была найдена ранее
                        if(!foundGroups.Contains(valueAtt.Number))
                        {
                            found++;
                            foundGroups.Add(valueAtt.Number);
                            groups.Add(new TagGroup(valueAtt.Number, folder)); //добавляем новую группу
                        }
                    }
                    i++;
                }
            }
            groups = groups.OrderBy((g) => g.GroupNumber).ToList();
        }

        #region ISerializable Members

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("folder", folder);
            using (FileStream fs1 = File.Create(string.Format("{0}/{1}", folder, "nGramm.mdl")))
            using (FileStream fs2 = File.Create(string.Format("{0}/{1}", folder, "entClass.mdl")))
            {
                Serializer.Serialize<TagNGramm>(fs1, nGramm);
                entClass.SaveTo(fs2);
            }
            info.AddValue("groups", groups, typeof(List<TagGroup>));
            info.AddValue("punctuation", punctuation, typeof(List<string>));
            info.AddValue("delimiters", sentenceDelimiters, typeof(List<string>));
            info.AddValue("lemmaFile", lemmaFile);
            info.AddValue("lexemPattern", lexemPattern, typeof(Regex));
            info.AddValue("sentencePattern", sentencePattern, typeof(Regex));
            info.AddValue("minLenght", minLengh);
            info.AddValue("serviceTags", serviceTags);
        }

        #endregion
    }
}
