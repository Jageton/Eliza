using System;
using System.Runtime.Serialization;

namespace ELIZA.Morphology.Dawg
{
    [Serializable]
    public class DawgException : Exception
    {
        public DawgException()
        {
        }
        public DawgException(string message) : base(message)
        {
        }
        public DawgException(string message, Exception inner) : base(message, inner)
        {
        }
        protected DawgException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
