using System;
using System.Runtime.Serialization;

namespace CS_Ex1
{
    [Serializable]
    internal class NoBookSelectedException : Exception
    {
        public NoBookSelectedException() : base("No Book Selected")
        {
          
        }

        public NoBookSelectedException(string message) : base(message)
        {
        }

        public NoBookSelectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoBookSelectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}