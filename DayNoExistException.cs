using System;
using System.Runtime.Serialization;

namespace PayWork
{
    [Serializable]
    public class DayNoExistException : Exception
    {
        public DayNoExistException()
        {
        }

        public DayNoExistException(string message) : base(message)
        {
        }

        public DayNoExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DayNoExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}