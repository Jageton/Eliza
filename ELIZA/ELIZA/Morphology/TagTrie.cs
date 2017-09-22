using System.IO;
using ProtoBuf;

namespace ELIZA.Morphology
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Morphology.SparseStringTrie{System.UInt16}" />
    [ProtoContract]
    [ProtoInclude(111111, typeof(TrieEntropyClassModel))]
    public class TagTrie: SparseStringTrie<ushort>
    {
        /// <summary>
        /// Сохраняет модель в заданный поток.
        /// </summary>
        /// <param name="fs">Поток.</param>
        public virtual void SaveTo(Stream fs)
        {
            BinaryWriter bw = new BinaryWriter(fs);
            Serialize(bw, (SparseNode<ushort>)this.root);
        }
        /// <summary>
        /// Алгоритм сериализации n-арного дерева.
        /// </summary>
        /// <param name="bw">Поток.</param>
        /// <param name="root">Корень сериализуемого дерева.</param>
        private void Serialize(BinaryWriter bw, SparseNode<ushort> root)
        {
            if (root == null)
            {
                bw.Write("#");
                return;
            }
            bw.Write(root.Key);
            bw.Write(root.HasValue);
            if (root.HasValue)
                bw.Write(root.Value);
            Serialize(bw, (SparseNode<ushort>)root.LeftChild);
            Serialize(bw, (SparseNode<ushort>)root.RightSibling);
        }
        /// <summary>
        /// Загружает модель из заданого потока.
        /// </summary>
        /// <param name="fs">Поток.</param>
        public virtual void Load(Stream fs)
        {
            BinaryReader br = new BinaryReader(fs);
            this.root = Deserialize(br);
        }
        /// <summary>
        /// Алгоритм десериализации n-арного дерева.
        /// </summary>
        /// <param name="br">Поток.</param>
        /// <returns>Возвращает корень десериалзованного дерева.</returns>
        private SparseNode<ushort> Deserialize(BinaryReader br)
        {
            if (br.BaseStream.Position == br.BaseStream.Length)
                return null;
            string key = br.ReadString();
            if (key == "#")
                return null;
            SparseNode<ushort> result = new SparseNode<ushort>();
            result.Key = key;
            result.HasValue = br.ReadBoolean();
            if (result.HasValue)
                result.Value = br.ReadUInt16();
            result.LeftChild = Deserialize(br);
            result.RightSibling = Deserialize(br);
            return result;
        }
    }
}
