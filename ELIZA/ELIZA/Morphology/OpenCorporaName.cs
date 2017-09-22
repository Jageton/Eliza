using System;
using ProtoBuf;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Задаёт псевдоним модели open corpora.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [System.AttributeUsage(System.AttributeTargets.Field | AttributeTargets.Enum)]
    [Serializable]
    [ProtoContract]
    public class OpenCorporaName: System.Attribute
    {
        private string name;
        
        /// <summary>
        /// Получает или задаёт псевдоним для модели open corpora.
        /// </summary>
        [ProtoMember(1)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="OpenCorporaName"/>.
        /// </summary>
        /// <param name="name">Псевдоним модели open corpora.</param>
        public OpenCorporaName(string name)
        {
            this.name = name;
        }
    }
}
