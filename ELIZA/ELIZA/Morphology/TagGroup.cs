using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using libsvm;
using System.IO;
using System.Security.Permissions;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Представляет собой морфологическую группу. Содержит классификаторы для всех 
    /// подтэгов группы, работающие по принципу один-против-всех.
    /// </summary>
    /// <seealso cref="System.Runtime.Serialization.ISerializable" />
    [Serializable]
    public class TagGroup : ISerializable
    {
        //все значения группы
        private Tag fullGroup;
        //номер группы
        private int groupNumber;
        //список подтэгов 
        private List<Tag> tags;
        private C_SVC classifier;
        private StreamWriter writer;
        //папка, в которой будут храниться обученные классификаторы и использованные выборки
        private string folder;
        //TODO: использовать методы оптимизация вместо поиска по сетке
        //возможные значения погрешности
        private static double[] cGrid = {1E-3, 1E-2, 1E-1, 1, 1E1, 2E1, 5E1, 1E2, 5E2, 1E3, 5E3, 1E4, 1E5, 1E6};
        //возможные значения близости различных векторов
        private static double[] gammaGrid = {1E-5, 1E-5, 1E-4, 1E-3, 1E-2, 1E-1};
        //количество векторов
        private int vectorCnt;

        /// <summary>
        /// Получает или задаёт тэг, содержащий все подтэги группы и только их.
        /// </summary>
        public Tag FullGroup
        {
            get { return fullGroup; }
            set { fullGroup = value; }
        }
        /// <summary>
        /// Получает или задаёт .
        /// </summary>
        public int GroupNumber
        {
            get { return groupNumber; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TagGroup"/>.
        /// </summary>
        /// <param name="groupNum">Номер группы.</param>
        /// <param name="folder">Папка, в которой будет создан классификатор.</param>
        public TagGroup(int groupNum, string folder)
        {
            groupNumber = groupNum;
            tags = new List<Tag>();
            vectorCnt = 0;
            this.folder = folder;
            Initialize();
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TagGroup"/>.
        /// </summary>
        public TagGroup()
        {
            groupNumber = 0;
            classifier = null;
            folder = "";
            vectorCnt = 0;
        }
        /// <summary>
        /// Инциализирует новый экземпляр класса <see cref="TagGroup"/>.
        /// </summary>
        /// <param name="si">Информация о сериализации.</param>
        /// <param name="context">Контекст.</param>
        protected TagGroup(SerializationInfo si, StreamingContext context)
        {
            fullGroup = (Tag)si.GetValue("fullGroup", typeof(Tag));
            groupNumber = si.GetInt32("groupNumber");
            folder = si.GetString("folder");
            vectorCnt = si.GetInt32("vectorCnt");
            tags = (List<Tag>)si.GetValue("tags", typeof(List<Tag>));
            classifier = new C_SVC(string.Format("{0}/classifier.mdl", folder));
        }


        /// <summary>
        /// Добавляет новый вектор к текущей модели.
        /// Вектор будет учавствовать в обучении модели.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="tag">Правильный тэг.</param>
        public void AddVector(double[] vector, Tag tag)
        {
            vectorCnt++;
            if (ContainsGroup(tag))
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    if ((tags[i] & tag) != 0)
                    {
                        WriteVector(vector, (i + 1).ToString(), writer);
                    }
                }
            }
            else WriteVector(vector, "0", writer);
        }
        /// <summary>
        /// Запускает процесс обучения модели.
        /// </summary>
        /// <remarks>Процесс может занять долгое время.</remarks>
        public void Train(bool concurrent)
        {
            writer.Close();
            CreateParameterSet();
            svm_parameter parameters = EvaluateParameters();
            var problem = ProblemHelper.ReadProblem(string.Format("{0}/training.set", folder));
            classifier = new C_SVC(problem, KernelHelper.RadialBasisFunctionKernel(parameters.gamma),
                parameters.C, 100D, true);
            classifier.Train();
            File.Delete(string.Format("{0}\\training.set", folder));
            File.Delete(string.Format("{0}\\training.par", folder));
        }
        /// <summary>
        /// Использует обученную модель для предугадывания правильного подтэга по вектору.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <returns>Возвращает угаданынй подтэг.</returns>
        /// <remarks>Вектор должен быть построен тем же образом, что и вектора для 
        /// обучения.</remarks>
        public Tag Predict(double[] vector)
        {
            var list = new List<svm_node>();            
            for (var i = 0; i < vector.Length; i++) //копируем вектор
            {
                if (vector[i] != 0)
                {
                    var node = new svm_node();
                    node.index = i + 1;
                    node.value = vector[i];
                    list.Add(node);
                }
            }
            var nodes = list.ToArray(); //вектор в представлении libsvm
            int index = (int)classifier.Predict(nodes) - 1;
            if (index == -1)
                return Tag.NoWord;
            return tags[index];
        }
        /// <summary>
        /// Определяет, содержит ли заданный тэг ровно один подтэг из группы.
        /// </summary>
        /// <param name="tag">Тестируемый тэг.</param>
        /// <returns>Возвращает <c>true</c>, если заданный тэг, содержит ровно один подтэг 
        /// из группы, иначе - <c>false</c>.</returns>
        public bool ContainsOneFromGroup(Tag tag)
        {
            return tags.Count(item => (item & tag) != 0) == 1;
        }
        /// <summary>
        /// Определяет, содержит ли заданный тэг подтэг из группы.
        /// </summary>
        /// <param name="tag">Тестируемый тэг.</param>
        /// <returns>Возвращает <c>true</c>, если заданный тэг, содержит подтэг 
        /// из группы, иначе - <c>false</c>.</returns>
        public bool ContainsGroup(Tag tag)
        {
            return (fullGroup & tag) != 0;
        }

        /// <summary>
        /// Инициализирует данный объект значениями по умолчанию.
        /// </summary>
        private void Initialize()
        {
            fullGroup = Tag.NoWord;
            bool append = true;
            tags = new List<Tag>();
            foreach (var item in Enum.GetValues(typeof(Tag)))
            {
                Enum value = (Enum)item;
                var memberInfo = typeof(Tag).GetField(value.ToString());
                var attr = memberInfo.GetCustomAttribute<MorphologyGroup>();
                if (attr != null)
                {
                    string name = attr.Name;
                    int number = attr.Number;
                    if (groupNumber == number)
                    {
                        fullGroup |= (Tag)value;
                        tags.Add((Tag)value);
                        if (append)
                        {
                            folder += "\\" + name;
                            append = false;
                        }
                    }
                }
            }
            Directory.CreateDirectory(folder);
            writer = File.CreateText(string.Format("{0}\\training.set", folder));
        }
        /// <summary>
        /// Записывает заданный вектор в формате libsvm, используя заданную метку 
        /// (имеет значение +1 или -1) и файловый дескриптор.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="label">Метка.</param>
        /// <param name="streamWriter">Файловый дескриптор.</param>
        private void WriteVector(double[] vector, string label, StreamWriter streamWriter)
        {
            string line = label + " ";
            for(int i = 0; i < vector.Length; i++)
            {
                if(vector[i] != 0)
                {
                    line += string.Format("{0}:{1} ", i + 1, vector[i]);
                }
            }
            streamWriter.WriteLine(line);
        }
        /// <summary>
        /// Находит лучшую комбинацию параметров модели, при которой достигается 
        /// наилучшая точность.
        /// </summary>
        /// <returns>Возвращает наилучшие параметры для построения модели.</returns>
        private svm_parameter EvaluateParameters()
        {
            svm_parameter parameter = new svm_parameter();
            parameter.kernel_type = (int)KernelType.RBF;
            parameter.shrinking = 1;
            parameter.probability = 1;
            var parProblem = ProblemHelper.ReadProblem(string.Format("{0}/training.par", folder));
            double bestAcc = 0;
            double bestGamma = double.MinValue;
            double bestC = double.MinValue;
            //перебираем все пары параметров
            for (int i = 0; i < cGrid.Length; i++)
            {
                for (int j = 0; j < gammaGrid.Length; j++)
                {
                    parameter.C = cGrid[i];
                    parameter.gamma = gammaGrid[j];
                    //измеряем точность с помощью кросс-валидации
                    var svm = new C_SVC(parProblem,
                        KernelHelper.RadialBasisFunctionKernel(parameter.gamma), parameter.C);
                    double currAcc = svm.GetCrossValidationAccuracy(5);
                    if (currAcc > bestAcc)
                    {
                        bestAcc = currAcc;
                        bestGamma = parameter.gamma;
                        bestC = parameter.C;
                    }
                }
            }            
            parameter.C = bestC;
            parameter.gamma = bestGamma;
            using(StreamWriter sw = new StreamWriter(File.Open("log.txt", FileMode.Append)))
            {
                sw.WriteLine("group: {0}, gamma: {1}, C: {2},  prediction: accuracy:{3}",
                    groupNumber, bestGamma, bestC, Math.Round(bestAcc, 2));
            }
            return parameter;
        }
        /// <summary>
        /// Создаёт небольшую выборку для нахождения наилучших параметров.
        /// </summary>
        private void CreateParameterSet()
        {
            using(StreamReader reader = File.OpenText(string.Format("{0}/training.set", folder)))
            using(StreamWriter parameterWriter = File.CreateText(string.Format("{0}/training.par", folder)))
            {
                int part = 5;
                int parSetSize = vectorCnt / part;
                for(int i = 0; i < parSetSize; i++)
                {
                    //перепишем заданное количество векторов из исходного файла в тестовый
                    parameterWriter.WriteLine(reader.ReadLine()); 
                }
            }
        }

        #region ISerializable Members

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("fullGroup", fullGroup);
            info.AddValue("groupNumber", groupNumber);
            info.AddValue("folder", folder);
            info.AddValue("vectorCnt", vectorCnt);
            info.AddValue("tags", tags);
            classifier.Export(string.Format("{0}/classifier.mdl", folder));
        }

        #endregion
    }
}
