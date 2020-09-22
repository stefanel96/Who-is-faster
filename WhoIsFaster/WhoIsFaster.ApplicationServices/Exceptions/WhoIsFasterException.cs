using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsFaster.ApplicationServices.Exceptions
{
    public class WhoIsFasterException : Exception
    {
        public WhoIsFasterException()
        {
        }

        public WhoIsFasterException(string message) : base(message)
        {
        }
    }
}
