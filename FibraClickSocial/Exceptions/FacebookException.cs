using System;

namespace FibraClickSocial.Exceptions
{
    class FacebookException : Exception
    {
        public FacebookException(string message) : base(message)
        {
        }

        public FacebookException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
