using System;
using ProtoBuf;

namespace ELIZA.Morphology
{
    /// <summary>
    /// В представлении тэга играет роль разбиение на морфологические группы, такие как 
    /// падеж, число, род и т.д. При применении к перечислению имеет другую семантику: 
    /// показывает общее количество групп в данном перечислении.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [ProtoContract]
    [Serializable]
    public class MorphologyGroup: Attribute
    {
        private int groupNum;
        private string groupName;

        /// <summary>
        /// Получает или задаёт номер группы - уникальное число в рамках данного перчисления.
        /// При применении к перечислению показывает общее количество групп.
        /// </summary>
        [ProtoMember(1)]
        public int Number
        {
            get { return groupNum; }
            set { groupNum = value; }
        }
        /// <summary>
        /// Получает или задаёт название группы. При применении к перечислению игнорируется.
        /// </summary>
        [ProtoMember(2)]
        public string Name
        {
            get { return groupName; }
            set { groupName = value; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MorphologyGroup"/>.
        /// </summary>
        /// <param name="groupNum">Номер группы.</param>
        /// <param name="name">Название группы.</param>
        public MorphologyGroup(int groupNum, string name)
        {
            Number = groupNum;
            Name = name;
        }
    }
}
