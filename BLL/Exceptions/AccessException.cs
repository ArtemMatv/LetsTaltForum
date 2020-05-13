using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class AccessException : Exception
    {
        public AccessException()
        {
        }

        public AccessException(string message)
            : base(message)
        {

        }

        public AccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
