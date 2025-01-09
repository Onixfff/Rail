using System;

namespace rail.Models.Error
{
    public class ErrorUpdateMoveMas : Exception
    {
        public string ErrorMessage { get; private set; }
        public string ErrorCode { get; private set; }
        private Exception _ex;

        public ErrorUpdateMoveMas(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
            ErrorMessage = message;
        }

        public ErrorUpdateMoveMas(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            ErrorMessage = message;
            _ex = innerException;
        }

        public Exception GetException()
        {
            if (_ex != null)
            {
                return _ex;
            }
            else
            {
                return null;
            }
        }
    }
}
