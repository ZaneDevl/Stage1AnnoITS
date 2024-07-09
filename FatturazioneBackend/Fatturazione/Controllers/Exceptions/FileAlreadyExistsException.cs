using Microsoft.Identity.Client;
using System;

namespace Fatturazione.Exceptions
{
    public class FileAlreadyExistsException : Exception
    {
        public FileAlreadyExistsException(string message) : base (message)
        {

        }
    }
}
