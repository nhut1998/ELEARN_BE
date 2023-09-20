using System;

namespace DecentralizedSystem.Middlewares
{
    [Serializable]
    public class CustomException : Exception
    {
        public string ErrorCode = "";

        public CustomException() : base()
        {
        }
        public CustomException(string message) : base(message)
        {
            this.ErrorCode = "ERROR_UNHANDLER";
        }

        public CustomException(string code, string message) : base(message)
        {
            this.ErrorCode = code;
        }
        public CustomException(string code, string message, Exception inner) : base(message, inner)
        {
            this.ErrorCode = code;
        }
    }
}
